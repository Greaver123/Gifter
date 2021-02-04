using Gifter.Common.Extensions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.DTOs.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class ImageService : IImageService
    {
        private readonly GifterDbContext dbContext;
        private readonly IFilesService filesService;
        private readonly StoreOptions options;

        public ImageService(GifterDbContext dbContext, IOptions<StoreOptions> options, IFilesService filesService)
        {
            this.dbContext = dbContext;
            this.filesService = filesService;
            this.options = options.Value;
        }

        public async Task<int> UploadAsync(UploadImageDTO uploadImageDTO, string userId)
        {
            if (uploadImageDTO == null) throw new ArgumentNullException(nameof(uploadImageDTO));
            
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"{nameof(userId)} is null or empty.");

            try
            {
                var fileFullPath = await filesService.StoreImageAsync(uploadImageDTO.ImageFile, userId);
                var result = await AddImage(uploadImageDTO.WishId, fileFullPath, userId);
            }
            catch (IOException ex)
            {
                //TODO
            }

            return 1;
        }
        /// <summary>
        /// Save Image for given WishId
        /// </summary>
        /// <param name="wishId">Id of wish entity</param>
        /// <param name="filePath">Path for image</param>
        /// <param name="userId">Authenticated userid</param>
        /// <returns>true if operation succeded</returns>
        public async Task<bool> AddImage(int wishId, string filePath, string userId)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException($"{nameof(filePath)} is null or empty.");
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"{nameof(userId)} is null or empty.");

            var wish = await dbContext.Wishes
                .Include(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.Id == wishId && w.WishList.User.Auth0Id == userId);

            if (wish == null) return false;

            if (wish.Image == null)
            {
                wish.Image = new Image() { Path = filePath };
            }
            else
            {
                var oldImagePath = wish.Image.Path;
                wish.Image.Path = filePath;
                File.Delete(oldImagePath);
            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                File.Delete(filePath);
                throw;
            }

            return true;
        }

        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object</param>
        /// <param name="dirName">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="IOException">Thrown when file already exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when formFile or dirName is null/empty or whitespace</exception>
        /// <exception cref="ArgumentException">Thrown when formFile is not a image.</exception>
        /// 
        public async Task<string> SaveImageAsync(IFormFile formFile, string dirName)
        {
            if (formFile == null) throw new ArgumentNullException(nameof(formFile));
            if (string.IsNullOrWhiteSpace(dirName)) throw new ArgumentNullException($"{dirName} cannot be null, empty or whitespace.");
            if (formFile.IsImage()) throw new ArgumentException("File is not a image.");
            if (formFile.FileName.Length > options.FileMaxSize * 1000000) throw new ArgumentException("File is too big.");

            //var name = $"{Path.GetRandomFileName().Replace(".", "")}{DateTime.Now.Ticks}";
            var name = $"{DateTime.Now.Ticks}";
            var userDir = $"{this.options.BaseDirectory}\\{dirName}";
            var extension = formFile.TryGetImageExtension();

            if (extension == null) throw new ArgumentException("File signature not recognised.");

            var fileFullpath = $"{userDir}\\{name}{extension}";

            if (File.Exists(fileFullpath)) throw new IOException($"File already exists with given name: {name}.");

            Directory.CreateDirectory(userDir);

            using (var stream = File.Create(fileFullpath))
            {
                await formFile.CopyToAsync(stream);
            }

            return fileFullpath;
        }

        public async Task<ImageDTO> GetImage(int id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
