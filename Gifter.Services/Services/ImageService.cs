using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.DTOs.Image;
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

        public ImageService(GifterDbContext dbContext, IOptions<StoreOptions> options)
        {
            this.dbContext = dbContext;
            this.options = options.Value;
        }

        /// <summary>
        /// Save Image for given WishId
        /// </summary>
        /// <param name="wishId">Id of wish entity</param>
        /// <param name="filePath">Path for image</param>
        /// <param name="userId">Authenticated userid</param>
        /// <returns>true if operation succeded</returns>
        /// <exception cref="ArgumentException">Thrown when filepath are null/empty or whitespace</exception>
        /// <exception cref="DbUpdateException">Thrown when Wish with provided wishId does not exist.</exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        public async Task<bool> AddImageAsync(int wishId, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException($"{nameof(filePath)} is null or empty.");

            var image = await dbContext.Images
                .FirstOrDefaultAsync(i => i.WishId == wishId);

            var fileToDeletePath = string.Empty;

            if (image == null)
            {
                dbContext.Images.Add(new Image() { Path = filePath, WishId = wishId });
            }
            else
            {
                fileToDeletePath = image.Path;
                image.Path = filePath;
            }

            await dbContext.SaveChangesAsync();

            //DELETE old image from filesystem
            if (!string.IsNullOrWhiteSpace(fileToDeletePath) && File.Exists(fileToDeletePath))
            {
                File.Delete(fileToDeletePath);
            }

            return true;
        }

        public async Task<ImageDTO> GetImage(int id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
