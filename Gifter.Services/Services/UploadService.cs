using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.Services.DTOS.Image;
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

        public async Task<bool> UploadAsync(UploadImageDTO uploadImageDTO, string userId)
        {
            if (uploadImageDTO == null) throw new ArgumentNullException(nameof(uploadImageDTO));
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"{nameof(userId)} is null or empty.");

            bool result = false;

            try
            {
                var wish = await dbContext.Wishes
                    .Include(w => w.WishList)
                        .ThenInclude(wl => wl.User)
                    .FirstOrDefaultAsync(w => w.Id == uploadImageDTO.WishId && w.WishList.User.Auth0Id == userId);

                if (wish == null) return result;


                var fileFullPath = await filesService.StoreImageAsync(uploadImageDTO.ImageFile, userId);
                result = await imageService.AddImageAsync(uploadImageDTO.WishId, fileFullPath);
            }
            catch (Exception ex)
            {
                // known exceptions
                if (
                    ex is FormatException ||
                    ex is FileSizeException ||
                    ex is IOException ||
                    ex is DbUpdateException)
                {

                    throw new UploadFileException($"Could not upload file. {ex.Message}", ex);
                }
                //TODO something went wrong. Try again.
                throw;
            }

            return result;
        }

        /// <summary>
        /// Gets image for given wishId.
        /// </summary>
        /// <param name="wishId">Id of wish.</param>
        /// <returns>Image byte array or null if there is no image for given <paramref name="wishId"/></returns>
        /// <exception cref="ArgumentException">Thrown when userId is null, empty or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when could not find file on filesystem which path is stored in Wish entity.</exception>
        /// <exception cref="FileLoadException">Thrown when could not load file extension from Image entity path.</exception>
        public async Task<DownloadImageDTO> DownloadImageAsync(int wishId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"{nameof(userId)} is null, empty or whitespace.");

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == wishId && w.WishList.User.Auth0Id == userId);

            if (wish == null || wish.Image == null) return null;

            var fileExtension = Path.GetExtension(wish.Image.Path);

            if (string.IsNullOrWhiteSpace(fileExtension)) throw new FileLoadException($"Could not load file extension from filepath: {wish.Image.Path}.");

            return new DownloadImageDTO()
            {
                FileExtension = fileExtension.Remove(0,1), //Remove "." from extension string
                Image = await filesService.GetStoredImageAsync(wish.Image.Path)
            };
        }
    }
}
