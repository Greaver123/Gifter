﻿using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.Constants;
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

        public async Task<OperationResult<WishlistCreateDTO>> CreateWishlist(string title, string userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == userId);

            var operationResult = new OperationResult<WishlistCreateDTO>();

            if (user == null)
            {
                operationResult.Message = "User with given id does not exist.";
                operationResult.Status = OperationStatus.Fail;
                operationResult.Payload = null;

                return operationResult;
            }

            var wishlist = new WishList() { Name = title, User = user };
            dbContext.Wishlists.Add(wishlist);

            var entitiesAffected = await dbContext.SaveChangesAsync();

            operationResult.Message = "Wishlist created";
            operationResult.Status = OperationStatus.Success;

            return new OperationResult<WishlistCreateDTO>()
            {
                Payload = new WishlistCreateDTO() { Id = wishlist.Id, Title = wishlist.Name },
                Status = OperationStatus.Success,
                Message = "Wishlist Created"
            };

        }

        public async Task<bool> DeleteWishlist(int id, string userId)
        {
            var wishlist = await dbContext.Wishlists
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id && w.User.Auth0Id == userId);

            if (wishlist != null)
            {
                dbContext.Wishlists.Remove(wishlist);
                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public int EditWishlit(WishlistEditDTO wishlistEditDTO, string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<WishlistDTO> GetWishlist(int id, string userid)
        {
            var wishlist = await dbContext.Wishlists
                .Include(w => w.Gifts)
                .Include(w => w.User)
                .Include(w => w.GiftGroup)
                    .ThenInclude(g => g.Event)
                .FirstOrDefaultAsync(w => w.User.Auth0Id == userid && id == w.Id);

            var wishlistDTO = new WishlistDTO() { Wishes = new List<WishDTO>() };

            if (wishlist != null)
            {
                foreach (var gift in wishlist.Gifts)
                {
                    wishlistDTO.Wishes.Add(new WishDTO()
                    {
                        Id = gift.Id,
                        Name = gift.Name,
                        Link = gift.URL,
                        Price = gift.Price
                    });
                }

                wishlistDTO.Title = wishlist.Name;
                wishlistDTO.GiftGroupName = wishlist.GiftGroup?.Name;
                wishlistDTO.EventDate = wishlist.GiftGroup?.Event?.Date;
            }

            return wishlistDTO;
        }

        public async Task<IEnumerable<WishlistPreviewDTO>> GetWishlists(string userId)
        {
            var wishlists = await dbContext.Wishlists
                .Include(w => w.User)
                .Where(w => w.User.Auth0Id == userId)
                .ToListAsync();

            var wishlistsDtos = new List<WishlistPreviewDTO>();

            foreach (var wishlist in wishlists)
            {
                wishlistsDtos.Add(new WishlistPreviewDTO()
                {
                    Id = wishlist.Id,
                    Title = wishlist.Name,
                    Assigned = wishlist.GiftGroupId != null
                });
            }

            return wishlistsDtos;
        }
    }
}
