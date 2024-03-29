﻿using Gifter.Common;
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
    public class ImageService : IImageService
    {
        private readonly GifterDbContext dbContext;
        private readonly StoreOptions options;
        private readonly IFilesService filesService;

        public ImageService(GifterDbContext dbContext,
            IOptions<StoreOptions> options,
            IFilesService filesService)
        {
            this.dbContext = dbContext;
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
        public async Task<OperationResult<ImageDTO>> UploadImageAsync(UploadImageDTO uploadImageDTO, string userId)
        {
            Guard.IsNull(uploadImageDTO, nameof(uploadImageDTO));
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == uploadImageDTO.WishId && w.WishList.User.Auth0Id == userId);

            if (wish == null) return new OperationResult<ImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Wish), uploadImageDTO.WishId),
            };

            try
            {
                var fileName = await filesService.StoreImageAsync(uploadImageDTO.ImageFile, userId, wish.WishList.DirectoryName);
                var fileToDelete = string.Empty;

                if (wish.Image == null)
                {
                    wish.Image = new Image() { FileName = fileName };
                }
                else
                {
                    fileToDelete = wish.Image.FileName;
                    wish.Image.FileName = fileName;
                }

                await dbContext.SaveChangesAsync();

                //Delete old file
                if (!string.IsNullOrWhiteSpace(fileToDelete)) filesService.DeleteImage(userId, wish.WishList.DirectoryName, fileToDelete);
            }
            catch (Exception ex)
            {
                var operationFailResult = new OperationResult<ImageDTO>()
                {
                    Status = OperationStatus.ERROR,
                    Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.create)
                };

                if (ex is FileServiceException) return operationFailResult;
                else if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    //File was saved on filesystem so do cleanup and delete that file

                    filesService.DeleteImage(userId, wish.WishList.DirectoryName, wish.Image.FileName);

                    return operationFailResult;
                }

                throw;
            }

            return new OperationResult<ImageDTO>()
            {
                Status = OperationStatus.SUCCESS,
                Message = MessageHelper.CreateOperationSuccessMessage(nameof(Image), OperationType.create),
                Data = new ImageDTO() { Id = wish.Image.Id,WishId = wish.Id}
                
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

            if (wish == null) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Wish), wishId)
            };

            if (wish.Image == null) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = $"There is no image for {nameof(Wish)} with id = {wishId}"
            };

            var filePath = $"{options.BaseDirectory}\\{userId}\\{wish.Image.FileName}";
            var fileExtension = Path.GetExtension(filePath);

            if (string.IsNullOrWhiteSpace(fileExtension)) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.ERROR,
                Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.read)
            };

            try
            {
                var storedImage = await filesService.GetStoredImageAsync(userId, wish.WishList.DirectoryName, wish.Image.FileName);

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

        /// <summary>
        /// Gets image by id.
        /// </summary>
        /// <param name="wishId">Id of image</param>
        /// <param name="userId">Id of user</param>
        /// <returns><para>OperationResult "Success" with image in Base64 if image found.</para> <para> OperationResult "Fail" if Image/Wish not found. </para>
        /// <para>OperationResult "Error" if file corrupted and could not load file. </para></returns>
        public async Task<OperationResult<DownloadImageDTO>> GetImageAsync(int imageId, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));

            var image = await dbContext.Images
                .Include(i => i.Wish)
                .ThenInclude(wl => wl.WishList)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == imageId && i.Wish.WishList.User.Auth0Id == userId);

            if (image == null) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Image), imageId)
            };

            var filePath = $"{options.BaseDirectory}\\{userId}\\{image.FileName}";
            var fileExtension = Path.GetExtension(filePath);

            if (string.IsNullOrWhiteSpace(fileExtension)) return new OperationResult<DownloadImageDTO>()
            {
                Status = OperationStatus.ERROR,
                Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.read)
            };

            try
            {
                var storedImage = await filesService.GetStoredImageAsync(userId, image.Wish.WishList.DirectoryName, image.FileName);

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

        /// <summary>
        /// Deletes image by id.
        /// </summary>
        /// <param name="wishId">Id of image.</param>
        /// <param name="userId">Id of user.</param>
        /// <returns><para>OperationResult "Success" image found and deleted.</para> <para> OperationResult "Fail" if Image not found. </para>
        /// <para>OperationResult "Error" if problem connecting with db.</para></returns>
        public async Task<OperationResult<object>> DeleteImageAsync(int imageId, string userId)
        {
            var image = await dbContext.Images
                .Include(i => i.Wish)
                .ThenInclude(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .FirstOrDefaultAsync(i => i.Id == imageId && i.Wish.WishList.User.Auth0Id == userId);

            if (image == null) return new OperationResult<object>()
            {
                Status = OperationStatus.FAIL,
                Message = MessageHelper.CreateEntityNotFoundMessage(nameof(Image), imageId)
            };

            try
            {
                var wishlistDir = image.Wish.WishList.DirectoryName;
                var imageFileName = image.FileName;

                dbContext.Images.Remove(image);
                await dbContext.SaveChangesAsync();
                filesService.DeleteImage(userId, wishlistDir, imageFileName);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return new OperationResult<object>()
                    {
                        Status = OperationStatus.ERROR,
                        Message = MessageHelper.CreateOperationErrorMessage(nameof(Image), OperationType.delete)
                    };
                }
                else if (ex is FileServiceException)
                {
                    //Log and do nothing, most important is that it will disaper from UI.
                    return new OperationResult<object>()
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = MessageHelper.CreateOperationSuccessMessage(nameof(Image), OperationType.delete)
                    };
                }
                throw;
            }

            return new OperationResult<object>()
            {
                Status = OperationStatus.SUCCESS,
                Message = MessageHelper.CreateOperationSuccessMessage(nameof(Image), OperationType.delete)
            };
        }
    }
}
