using Gifter.Services.DTOS.Wish;
using Gifter.Services.DTOS.Wishlist;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IWishService
    {
        /// <summary>
        /// Deletes Wish from wishlist
        /// </summary>
        /// <param name="id">Id of wish to be deleted</param>
        /// <param name="userId">Id of user</param>
        /// <returns> True if deleted. False if wish could not be found for given userId.</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        Task<bool> DeleteAsync(int id, string userId);

        Task<int?> AddAsync(AddWishDTO addWishDTO, string userId);

        Task<WishDTO> GetAsync(int id, string userId);
    }
}