using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.Services.Common;
using Gifter.Services.DTOs.Image;
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
    }
}
