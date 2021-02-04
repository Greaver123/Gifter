using Gifter.Services.DTOs.Image;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IImageService
    {
        Task<int> UploadAsync(UploadImageDTO uploadImageDTO, string userId);

        /// <summary>
        /// Save Image for given WishId
        /// </summary>
        /// <param name="wishId">Id of wish entity</param>
        /// <param name="filePath">Path for image</param>
        /// <param name="userId">Authenticated userid</param>
        /// <returns>true if operation succeded</returns>
        /// <exception cref="DbUpdateException"></exception>
        Task<bool> AddImage(int wishId, string filePath, string userId);
    }
}