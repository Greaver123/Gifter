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
        /// Gets bytes array of stored image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns>Byte array of stored image</returns>
        /// <exception cref="FileServiceException">Thrown when could not get image from filesystem.</exception>
        Task<byte[]> GetStoredImageAsync(string imagePath);

        /// <summary>
        /// Delete image from filesystem for given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to file to be deleted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null, empty, or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when file could not be found or path is invalid</exception>
        void Delete(string path);

        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object</param>
        /// <param name="userId">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="FileServiceException">Thrown when could not store image due to corrupted user directory.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="formFile"/> is not a image.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="formFile"/> is too big.</exception>
        Task<string> StoreImageAsync(IFormFile formFile, string userId, int wishlistId);

        /// <summary>
        /// Create directory for given wishlist and user.
        /// </summary>
        /// <param name="name">Name of folder</param>
        /// <exception cref="ArgumentNullException">Thrown when path is null, empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when folder name is invalid</exception>
        /// <returns>Full path when directory created successful.</returns>
        string CreateDirectoryForWishlist(string wishlistId, string userId);

        /// <summary>
        /// Deletes wishlist folder with images. 
        /// </summary>
        /// <param name="wishlistId">Id/folder name of wishlist.</param>
        /// <param name="userId">Id/Folder name of user.</param>
        void DeleteWishlistStore(string wishlistId, string userId);
    }
}