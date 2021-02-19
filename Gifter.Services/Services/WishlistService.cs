using Gifter.Common;
using Gifter.Common.Exceptions;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.DTOS.Wishlist;
using Gifter.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly GifterDbContext dbContext;
        private readonly IFilesService filesService;

        public WishlistService(GifterDbContext dbContext, IFilesService filesService)
        {
            this.dbContext = dbContext;
            this.filesService = filesService;
        }

        public async Task<OperationResult<WishlistCreateDTO>> CreateWishlist(string title, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(title, nameof(title));
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == userId);
            var operationResult = new OperationResult<WishlistCreateDTO>();

            if (user == null)
                return new OperationResult<WishlistCreateDTO>
                {
                    Message = "Internal error: Could not retrive user info.",
                    Status = OperationStatus.ERROR,
                    Data = null
                };

            var wishlist = new WishList() { Name = title, User = user };
            dbContext.Wishlists.Add(wishlist);

            try
            {
                await dbContext.SaveChangesAsync();

                //Create directory for wishlist images
                filesService.CreateDirectoryForWishlist(wishlist.Id.ToString(), userId);
            }
            catch (Exception ex)
            {
                var operationErrorResult = new OperationResult<WishlistCreateDTO>()
                {
                    Status = OperationStatus.ERROR,
                    Message = MessageHelper.CreateOperationErrorMessage(nameof(WishList), OperationType.create),
                    Data = null
                };

                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return operationErrorResult;
                }
                else if (ex is FileServiceException)
                {
                    //Something went wrong when saving creating directory, so delete Wishlist entry from db
                    try
                    {
                        dbContext.Wishlists.Remove(wishlist);
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        //LOG that Could not make cleanup after FileServiceExcepion
                    }

                    return operationErrorResult;
                }

                //Bubble up rest to handle it in higher lvl
                throw;
            }

            return new OperationResult<WishlistCreateDTO>()
            {
                Data = new WishlistCreateDTO() { Id = wishlist.Id, Title = wishlist.Name },
                Status = OperationStatus.SUCCESS,
                Message = MessageHelper.CreateOperationSuccessMessage(nameof(WishList), OperationType.create)
            };
        }

        public async Task<OperationResult<object>> DeleteWishlist(int id, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wishlist = await dbContext.Wishlists
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id && w.User.Auth0Id == userId);

            if (wishlist == null) return new OperationResult<object>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(WishList), id),
            };

            try
            {
                dbContext.Wishlists.Remove(wishlist);
                await dbContext.SaveChangesAsync();
                
                filesService.DeleteWishlist(wishlist.Id, userId);
              
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return new OperationResult<object>()
                    {
                        Status = OperationStatus.ERROR,
                        Message = MessageHelper.CreateOperationErrorMessage(nameof(WishList), OperationType.delete),
                    };
                }
                else if(ex is FileServiceException)
                {
                    //Log and return success and for now ignore. Most important is that entry from db is deleted.
                    return new OperationResult<object>()
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = MessageHelper.CreateOperationSuccessMessage(nameof(WishList), OperationType.delete),
                    };
                }

                throw;
            }

            return new OperationResult<object>()
            {
                Status = OperationStatus.SUCCESS,
                Message = MessageHelper.CreateOperationSuccessMessage(nameof(WishList), OperationType.delete),
            };
        }

        /// <summary>
        /// Bulk edit for Wishlist
        /// </summary>
        /// <param name="wishlistEditDTO">Id of Wishlist</param>
        /// <param name="userId">Id of wishlist owner</param>
        /// <returns>OperationResult "Fail" if wishlist not found.</returns>
        public async Task<OperationResult<WishlistBulkEditDTO>> BulkEditWishlist(WishlistEditDTO wishlistEditDTO, string userId)
        {
            Guard.IsNull(wishlistEditDTO, nameof(wishlistEditDTO));
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wishlist = await dbContext.Wishlists
                .Include(w => w.User)
                .Include(w => w.Wishes)
                .Include(w => w.GiftGroup)
                .FirstOrDefaultAsync(w => w.User.Auth0Id == userId && w.Id == wishlistEditDTO.Id);

            if (wishlist == null)
                return new OperationResult<WishlistBulkEditDTO>
                {
                    Message = MessageHelper.CreateEntityNotFoundMessage(nameof(WishList), wishlistEditDTO.Id),
                    Status = OperationStatus.FAIL,
                    Data = null
                };

            //SET GIFTGROUP
            var giftgroup = await dbContext.GiftGroups.FirstOrDefaultAsync(g => g.Id == wishlistEditDTO.Id);
            wishlist.GiftGroup = giftgroup;

            //ADD NEW GIFTS OR UPDATE
            if (wishlist.Wishes != null)
            {
                foreach (var wish in wishlistEditDTO.Wishes)
                {
                    if (wish.IsNew)
                    {
                        wishlist.Wishes.Add(new Wish() { Name = wish.Name, URL = wish.Link, Price = wish.Price });
                    }
                    else
                    {
                        var giftToEdit = wishlist.Wishes.FirstOrDefault(g => g.Id == wish.Id);

                        if (giftToEdit != null)
                        {
                            giftToEdit.Name = wish.Name;
                            giftToEdit.URL = wish.Link;
                            giftToEdit.Price = wish.Price;
                        }
                    }
                }
            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return new OperationResult<WishlistBulkEditDTO>()
                    {
                        Status = MessageHelper.CreateOperationErrorMessage(nameof(WishList), OperationType.update),
                        Data = null,
                    };
                }
                throw;
            }

            return new OperationResult<WishlistBulkEditDTO>()
            {
                Status = OperationStatus.SUCCESS,
                Message = $"{nameof(WishList)} updated successful.",
                Data = new WishlistBulkEditDTO() { Id = wishlist.Id }
            };
        }

        public async Task<WishlistDTO> GetWishlist(int id, string userid)
        {
            var wishlist = await dbContext.Wishlists
                .Include(w => w.Wishes)
                .Include(w => w.User)
                .Include(w => w.GiftGroup)
                    .ThenInclude(g => g.Event)
                .FirstOrDefaultAsync(w => w.User.Auth0Id == userid && id == w.Id);

            var wishlistDTO = new WishlistDTO() { Wishes = new List<WishDTO>() };

            if (wishlist != null)
            {
                foreach (var wish in wishlist.Wishes)
                {
                    wishlistDTO.Wishes.Add(new WishDTO()
                    {
                        Id = wish.Id,
                        Name = wish.Name,
                        Link = wish.URL,
                        Price = wish.Price
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
