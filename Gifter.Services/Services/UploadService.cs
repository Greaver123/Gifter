using Gifter.Common;
using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Image;
using Gifter.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class UploadService : IUploadService
    {
        private readonly GifterDbContext dbContext;
        private readonly StoreOptions options;
        private readonly IImageService imageService;
        private readonly IFilesService filesService;

        public UploadService(GifterDbContext dbContext,
            IOptions<StoreOptions> options,
            IImageService imageService,
            IFilesService filesService)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.options = options.Value;
            this.filesService = filesService;
        }

        /// <summary>
        /// Upload file to db and filesystem for given user.
        /// </summary>
        /// <param name="uploadImageDTO">Image to be uploaded with wishlist id</param>
        /// <param name="userId">Id of user</param>
        /// <returns>OperationResult success if image saved on filesystem and in db. 
        /// <para>Fail if could not find wish for given id. </para><para> Error if some exception occured durning execution.</para></returns>
        public async Task<OperationResult<object>> UploadAsync(UploadImageDTO uploadImageDTO, string userId)
        {
            Guard.IsNull(uploadImageDTO, nameof(uploadImageDTO));
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .FirstOrDefaultAsync(w => w.Id == uploadImageDTO.WishId && w.WishList.User.Auth0Id == userId);

            if (wish == null) return new OperationResult<object>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Wish), uploadImageDTO.WishId),
            };

            var fileFullPath = string.Empty;
            try
            {
                fileFullPath = await filesService.StoreImageAsync(uploadImageDTO.ImageFile, userId, wish.WishListId);
                await imageService.AddImageAsync(uploadImageDTO.WishId, fileFullPath);
            }
            catch (Exception ex)
            {
                var operationFailResult = new OperationResult<object>()
                {
                    Status = OperationStatus.ERROR,
                    Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.create)
                };

                if (ex is FileServiceException) return operationFailResult;
                else if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    //File was saved on filesystem so do cleanup and delete that file
                    File.Delete(fileFullPath);
                    return operationFailResult;
                }

                throw;
            }

            return new OperationResult<object>()
            {
                Status = OperationStatus.SUCCESS,
                Message = MessageHelper.CreateOperationSuccessMessage(nameof(Image), OperationType.create)
            };
        }

        /// <summary>
        /// Gets image for given wishId.
        /// </summary>
        /// <param name="wishId">Id of wish.</param>
        /// <returns><para>OperationResult "Success" with image in Base64 if image for <paramref name="wishId"/> found.</para> <para> OperationResult "Fail" if Image/Wish not found. </para>
        /// <para>OperationResult "Error" if file corrupted and could not load file. </para></returns>
        public async Task<OperationResult<DownloadImageDTO>> DownloadImageAsync(int wishId, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == wishId && w.WishList.User.Auth0Id == userId);

            if (wish == null || wish.Image == null) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Wish), wishId)
            };

            if (wish.Image == null) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = $"There is no image for {nameof(Wish)} with id = {wishId}"
            };

            var fileExtension = Path.GetExtension(wish.Image.Path);

            if (string.IsNullOrWhiteSpace(fileExtension)) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.ERROR,
                Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.read)
            };

            try
            {
                var storedImage = await filesService.GetStoredImageAsync(wish.Image.Path);
               
                return new OperationResult<DownloadImageDTO>()
                {
                    Status = OperationStatus.SUCCESS,
                    Data = new DownloadImageDTO()
                    {
                        FileExtension = fileExtension.Remove(0, 1), //Remove "." from extension string
                        Image = "data:image/png;base64," + Convert.ToBase64String(storedImage)
                    }
                };
            }
            catch (FileServiceException)
            {
                return new OperationResult<DownloadImageDTO>()
                {
                    Status = OperationStatus.ERROR,
                    Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.read)
                };
            }
        }
    }
}
