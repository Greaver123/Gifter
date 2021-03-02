using Gifter.Services.Common;
using Gifter.Services.DTOs.Image;
using Gifter.Services.DTOS.Image;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IImageService
    {

        /// <summary>
        /// Deletes image by id.
        /// </summary>
        /// <param name="wishId">Id of image.</param>
        /// <param name="userId">Id of user.</param>
        /// <returns><para>OperationResult "Success" image found and deleted.</para> <para> OperationResult "Fail" if Image not found. </para>
        /// <para>OperationResult "Error" if problem connecting with db.</para></returns>
        Task<OperationResult<object>> DeleteImageAsync(int imageId, string userId);

        /// <summary>
        /// Gets image for given wishId.
        /// </summary>
        /// <param name="wishId">Id of wish.</param>
        /// <returns><para>OperationResult "Success" with image in Base64 if image for <paramref name="wishId"/> found.</para> <para> OperationResult "Fail" if Image/Wish not found. </para>
        /// <para>OperationResult "Error" if file corrupted and could not load file. </para></returns>
        Task<OperationResult<DownloadImageDTO>> DownloadImageAsync(int wishId, string userId);

        /// <summary>
        /// Gets image by id.
        /// </summary>
        /// <param name="wishId">Id of image</param>
        /// <param name="userId">Id of user</param>
        /// <returns><para>OperationResult "Success" with image in Base64 if image found.</para> <para> OperationResult "Fail" if Image/Wish not found. </para>
        /// <para>OperationResult "Error" if file corrupted and could not load file. </para></returns>
        Task<OperationResult<DownloadImageDTO>> GetImageAsync(int imageId, string userId);

        /// <summary>
        /// Upload file to db and filesystem for given user.
        /// </summary>
        /// <param name="uploadImageDTO">Image to be uploaded with wishlist id</param>
        /// <param name="userId">Id of user</param>
        /// <returns>OperationResult success if image saved on filesystem and in db. 
        /// <para>Fail if could not find wish for given id. </para><para> Error if some exception occured durning execution.</para></returns>
        Task<OperationResult<object>> UploadImageAsync(UploadImageDTO uploadImageDTO, string userId);
    }
}