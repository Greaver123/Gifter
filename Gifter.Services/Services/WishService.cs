using Gifter.Common;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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
        public async Task<OperationResult<WishDTO>> AddAsync(AddWishDTO addWishDTO, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNull(addWishDTO, nameof(addWishDTO));

            var wishlist = await dbContext.Wishlists
                .Include(wl => wl.User)
                .Include(wl => wl.Wishes)
                .FirstOrDefaultAsync(wl => wl.Id == addWishDTO.WishlistId && wl.User.Auth0Id == userId);

            if (wishlist == null) return new OperationResult<WishDTO>()
            {
                Data = null,
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(WishList), addWishDTO.WishlistId)
            };

            var wish = new Wish()
            {
                Name = addWishDTO.Name,
                Price = addWishDTO.Price,
                URL = addWishDTO.URL
            };

            wishlist.Wishes.Add(wish);
            await dbContext.SaveChangesAsync();

            return new OperationResult<WishDTO>()
            {
                Status = OperationStatus.SUCCESS,
                Data = new WishDTO()
                {
                    Id = wish.Id,
                    Name = wish.Name,
                    Link = wish.URL,
                    Price = wish.Price
                }
            };
        }

        /// <summary>
        /// Deletes Wish from wishlist
        /// </summary>
        /// <param name="id">Id of wish to be deleted</param>
        /// <param name="userId">Id of user</param>
        /// <returns> true if deleted. False if wish could not be found for given userId.</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        public async Task<OperationResult<bool>> DeleteAsync(int id, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var operatinResult = new OperationResult<bool>();

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == id && w.WishList.User.Auth0Id == userId);

            if (wish == null)
                return new OperationResult<bool>()
                {
                    Status = OperationStatus.FAIL,
                    Message = $"{nameof(Wish)} with id = {id} not found.",
                    Data = false
                };

            ////Check if there is image to be deleted from filesystem
            var imageToDeleteFilename = wish.Image?.FileName;

            dbContext.Wishes.Remove(wish);
            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(imageToDeleteFilename)) filesService.DeleteImage(userId, wish.WishList.DirectoryName, wish.Image.FileName);

            return new OperationResult<bool>()
            {
                Status = OperationStatus.SUCCESS,
                Data = true,
            };
        }

        /// <summary>
        /// Gets wish for given id.
        /// </summary>
        /// <param name="id">Id of wish.</param>
        /// <param name="userId">Id of user.</param>
        /// <returns>Operation result Success, when Wish with given id found. Otherwise operation result Fail.</returns>
        public async Task<OperationResult<WishDTO>> GetAsync(int id, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wish = await dbContext.Wishes
            .Include(w => w.Image)
                .Include(w => w.WishList)
            .ThenInclude(wl => wl.User)
            .FirstOrDefaultAsync(w => w.Id == id && w.WishList.User.Auth0Id == userId);

            return wish != null ? new OperationResult<WishDTO>()
            {
                Status = OperationStatus.SUCCESS,
                Data = new WishDTO()
                {
                    Id = wish.Id,
                    Name = wish.Name,
                    Link = wish.URL,
                    Price = wish.Price,
                    ImageId = wish.Image?.Id
                },
            } : new OperationResult<WishDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = $"{nameof(Wish)} with id = {id} not found.",
                Data = null
            };
        }


        /// <summary>
        /// Gets wish for given id.
        /// </summary>
        /// <param name="wishDTO">Wish to be edited</param>
        /// <param name="userId">Id of user.</param>
        /// <returns>Operation result Success, when Wish with given id succesfully update. Otherwise operation result Fail</returns>
        public async Task<OperationResult<UpdateWishDTO>> UpdateAsync(UpdateWishDTO wishDTO, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            
            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .FirstOrDefaultAsync(w => w.Id == wishDTO.Id && w.WishList.User.Auth0Id == userId);
            
            if(wish == null) return new OperationResult<UpdateWishDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = $"{nameof(Wish)} with id = {wishDTO.Id} not found.",
                Data = null
            };

            wish.Name = wishDTO.Name;
            wish.Price = wishDTO.Price;
            wish.URL = wishDTO.URL;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return new OperationResult<UpdateWishDTO>()
                    {
                        Status = OperationStatus.ERROR,
                        Message = MessageHelper.CreateOperationErrorMessage(nameof(Wish), OperationType.delete),
                        Data = null
                    };
                }

                throw;
            }

            return new OperationResult<UpdateWishDTO>()
            {
                Status = OperationStatus.SUCCESS,
                Data = new UpdateWishDTO()
                {
                    Id = wish.Id,
                    Name = wish.Name,
                    URL = wish.URL,
                    Price = wish.Price,
                },
            };
        }
    }
}
