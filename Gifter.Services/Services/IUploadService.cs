using Gifter.Services.DTOs.Image;
using Gifter.Services.DTOS.Image;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IUploadService
    {
        /// <summary>
        /// Gets image for given wishId.
        /// </summary>
        /// <param name="wishId">Id of wish.</param>
        /// <returns>Image byte array or null if there is no image for given <paramref name="wishId"/></returns>
        /// <exception cref="ArgumentException">Thrown when userId is null, empty or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when could not find file on filesystem which path is stored in Wish entity.</exception>
        /// <exception cref="FileLoadException">Thrown when could not load file extension from Image entity path.</exception>
        Task<DownloadImageDTO> DownloadImageAsync(int wishId, string userId);

        Task<bool> UploadAsync(UploadImageDTO uploadImageDTO, string userId);
    }
}