using Gifter.Services.Common;
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

        /// <summary>
        /// Upload file to db and filesystem for given user.
        /// </summary>
        /// <param name="uploadImageDTO">Image to be uploaded with wishlist id</param>
        /// <param name="userId">Id of user</param>
        /// <returns>OperationResult success if image saved on filesystem and in db. 
        /// <para>Fail if could not find wish for given id. </para><para> Error if some exception occured durning execution.</para></returns>
        Task<OperationResult<object>> UploadAsync(UploadImageDTO uploadImageDTO, string userId);
    }
}