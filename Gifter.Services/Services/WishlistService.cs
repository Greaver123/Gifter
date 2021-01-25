using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.DTOS.Wishlist;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly GifterDbContext dbContext;

        public WishlistService(GifterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<WishlistCreateDTO> CreateWishlist(string title, string userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == userId);

            if (user == null)
            {
                throw new System.Exception("User with given dd dose not exist. ");
            }

            var wishlist = new WishList() { Name = title, User = user };
            dbContext.Wishlists.Add(wishlist);
            await dbContext.SaveChangesAsync();

            return new WishlistCreateDTO() { Id = wishlist.Id, Title = wishlist.Name };

        }

        public async Task<bool> DeleteWishlist(int id, string authUserId)
        {
            var wishlist = await dbContext.Wishlists
                .Include(w=>w.User)
                .FirstOrDefaultAsync(w => w.Id == id && w.User.Auth0Id == authUserId);

            if(wishlist != null)
            {
                dbContext.Wishlists.Remove(wishlist);
                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public int EditWishlit(WishlistEditDTO wishlistEditDTO, string userid)
        {
            throw new System.NotImplementedException();
        }

        public WishlistDTO GetWishlist(int id, string userid)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<WishlistDTO>> GetWishlists(string userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == userId);

            var wishlists = await dbContext.Wishlists.Where(w => w.UserId == user.Id).ToListAsync();

            var wishlistsDtos = new List<WishlistDTO>();

            foreach (var wishlist in wishlists)
            {
                wishlistsDtos.Add(new WishlistDTO() { 
                    Id = wishlist.Id, 
                    Title = wishlist.Name, 
                    Assigned = wishlist.GiftGroupId != null });
            }

            return wishlistsDtos;
        }
    }
}
