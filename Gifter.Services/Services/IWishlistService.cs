using Gifter.Services.Common;
using Gifter.Services.DTOS.Wishlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IWishlistService
    {
        Task<OperationResult<WishlistCreateDTO>> CreateWishlist(string title, string userId);
        Task<IEnumerable<WishlistPreviewDTO>> GetWishlists(string userId);
        Task<bool> DeleteWishlist(int id, string userid);
        Task<bool> EditWishlist(WishlistEditDTO wishlistEditDTO, string userid);
        Task<WishlistDTO> GetWishlist(int id, string userid);
    }
}
