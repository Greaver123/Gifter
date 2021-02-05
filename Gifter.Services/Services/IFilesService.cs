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
        /// Store image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object</param>
        /// <param name="dirName">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="IOException">Thrown when file already exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when formFile or dirName is null/empty or whitespace</exception>
        /// <exception cref="ArgumentException">Thrown when formFile is not a image.</exception>
        Task<string> StoreImageAsync(IFormFile formFile, string dirName);
    }
}