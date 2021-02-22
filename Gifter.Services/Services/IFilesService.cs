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
        /// <exception cref="FileNotFoundException">Thrown when file not found</exception>
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
        /// <param name="dirName">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="IOException">Thrown when file already exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when formFile or dirName is null/empty or whitespace.</exception>
        /// <exception cref="FormatException">Thrown when formFile is not a image.</exception>
        /// <exception cref="FileSizeException">Thron when size of file exceed max size.</exception>
        Task<string> StoreImageAsync(IFormFile formFile, string dirName);

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