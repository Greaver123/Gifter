﻿using Gifter.Services.Common;
using Gifter.Services.DTOS.Wishlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IWishlistService
    {
        Task<OperationResult<WishlistCreateDTO>> CreateWishlist(string title, string userId);
        Task<OperationResult<object>> DeleteWishlist(int id, string userid);
        Task<OperationResult<WishlistBulkEditDTO>> BulkEditWishlist(WishlistEditDTO wishlistEditDTO, string userId);
        Task<OperationResult<WishlistDTO>> GetWishlist(int id, string userid);
        Task<OperationResult<IEnumerable<WishlistPreviewDTO>>> GetWishlists(string userId);
    }
}
