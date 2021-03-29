using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IFilesService
    {
        /// <summary>
        /// Deletes all unassigned images from filesystem without entry in DB for given user.
        /// </summary>
        /// <param name="userId">Id of user/name if user directory</param>
        /// <returns>True if succes</returns>
        /// <exception cref="ArgumentNullException">Thrown when userId is null, empty or whitespace.</exception>
        Task<bool> DeleteUnassignedImages(string userId);


        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object.</param>
        /// <param name="userId">Name of directory to store images.</param>
        /// <returns>FileName with exension.</returns>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters in null, empty or whitespace.</exception>
        /// <exception cref="FileServiceException">Thrown when could not store image due internal error.</exception>
        Task<string> StoreImageAsync(IFormFile formFile, string userId, string wishlistDirName);

        string CreateDirectoryForWishlist(string wishlistDirName);

        /// <summary>
        /// Deletes wishlist folder with images. 
        /// </summary>
        /// <param name="wishlistDirName">Directory name of wishlist.</param>
        /// <param name="userId">Id of user.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters is null, empty or whitespace.</exception>
        /// <exception cref="FileServiceException">Thrown when could not delete image due some internal exception.</exception>
        void DeleteWishlistDirectory(string userId, string wishlistDirName);

        Task<byte[]> GetStoredImageAsync(string userId, string wishlitId, string fileName);

        void DeleteImage(string userId, string wishlistDirName, string fileName);
    }
}