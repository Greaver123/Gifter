using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IFilesService
    {
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