using Gifter.Common;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.DTOS.Wishlist;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class WishService : IWishService
    {
        private readonly GifterDbContext dbContext;
        private readonly IFilesService filesService;

        public WishService(GifterDbContext dbContext, IFilesService filesService)
        {
            this.dbContext = dbContext;
            this.filesService = filesService;
        }
        /// <summary>
        /// Adds new wish for given wishlistId
        /// </summary>
        /// <param name="addWishDTO">Wish to be added</param>
        /// <param name="userId">Id of user</param>
        /// <returns>Id of added wish or null if wishlist not found</returns>
        public async Task<int?> AddAsync(AddWishDTO addWishDTO, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNull(addWishDTO, nameof(addWishDTO));

            var wishlist = await dbContext.Wishlists
                .Include(wl => wl.User)
                .FirstOrDefaultAsync(wl => wl.Id == addWishDTO.WishlistId && wl.User.Auth0Id == userId);

            if (wishlist == null) return null;

            var wish = new Wish()
            {
                Name = addWishDTO.Name,
                Price = addWishDTO.Price,
                URL = addWishDTO.URL
            };

            wishlist.Wishes.Add(wish);
            await dbContext.SaveChangesAsync();

            return wish.Id;
        }


        /// <summary>
        /// Deletes Wish from wishlist
        /// </summary>
        /// <param name="id">Id of wish to be deleted</param>
        /// <param name="userId">Id of user</param>
        /// <returns> true if deleted. False if wish could not be found for given userId.</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == id && w.WishList.User.Auth0Id == userId);

            if (wish == null) return false;

            //Check if there is image to be deleted from filesystem
            var imageToDeletePath = wish.Image?.Path;

            dbContext.Wishes.Remove(wish);
            await dbContext.SaveChangesAsync();

            if (imageToDeletePath != null) filesService.Delete(imageToDeletePath);

            return true;
        }
    }

    public class AddWishDTO
    {
        [Required]
        public int WishlistId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string URL { get; set; }
    }
}
