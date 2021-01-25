using Gifter.Services.DTOS.Wishlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IWishlistService
    {
        Task<WishlistCreateDTO> CreateWishlist(string title, string userId);
        Task<IEnumerable<WishlistDTO>> GetWishlists(string userId);
        Task<bool> DeleteWishlist(int id, string userid);
        int EditWishlit(WishlistEditDTO wishlistEditDTO, string userid);
        WishlistDTO GetWishlist(int id, string userid);
    }
}
