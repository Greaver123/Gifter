using Gifter.Common;
using Gifter.Common.Exceptions;
using Gifter.Common.Extensions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class FilesService : IFilesService
    {
        private readonly StoreOptions options;
        private readonly GifterDbContext dbContext;

        public FilesService(GifterDbContext dbContext, IOptions<StoreOptions> options)
        {
            this.options = options.Value;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Create directory for given path.
        /// </summary>
        /// <param name="name">Name of folder</param>
        /// <exception cref="ArgumentNullException">Thrown when path is null, empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when folder name is invalid</exception>
        /// <returns>Full path when directory created successful.</returns>
        public string CreateDirectoryForWishlist(string name, string userId)
        {
            var userDirPath = $"{options.BaseDirectory}\\{userId}";
            var fullDirPath = $"{userDirPath}\\{name}";

            try
            {
                Guard.IsValidDirName(userId);
                Guard.IsValidDirName(name);

                return Directory.CreateDirectory(fullDirPath).FullName;
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object</param>
        /// <param name="userId">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="FileServiceException">Thrown when could not store image due to corrupted user directory.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="formFile"/> is not a image.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="formFile"/> is to big.</exception>
        public async Task<string> StoreImageAsync(IFormFile formFile, string userId, int wishlistId)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNull(formFile);

            if (!formFile.IsImage()) throw new ArgumentException("File is not a image.");
            if (formFile.Length > options.FileMaxSize) throw new FormatException("File is too big.");
      
            try
            {
                var name = $"{DateTime.Now.Ticks}";
                var wishlistDir = $"{this.options.BaseDirectory}\\{userId}\\{wishlistId}";
                var extension = formFile.TryGetImageExtension();

                if (extension == null) throw new FormatException("File signature not recognised.");
                var fileFullpath = $"{wishlistDir}\\{name}{extension}";

                if (!Directory.Exists(wishlistDir)) throw new IOException($"Directory \"{wishlistDir}\" does not exists.");
                if (File.Exists(fileFullpath)) throw new IOException($"File already exists with given name: {name}.");

                using (var stream = File.Create(fileFullpath))
                {
                    await formFile.CopyToAsync(stream);
                }
                return fileFullpath;
            }
            catch (Exception ex)
            {
                throw new FileServiceException("Internal error. Could not store image due to corrupted user directory.", ex);
            }
        }

        /// <summary>
        /// Deletes all unassigned images from filesystem without entry in DB for given user.
        /// </summary>
        /// <param name="userId">Id of user/name if user directory</param>
        /// <returns>True if success</returns>
        /// <exception cref="ArgumentNullException">Thrown when userId is null, empty or whitespace.</exception>
        public async Task<bool> DeleteUnassignedImages(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));

            //Get all images for given user
            var imagesInDB = await dbContext.Images
                .Include(i => i.Wish)
                .ThenInclude(w => w.WishList)
                .ThenInclude(wl => wl.User)
                .Where(i => i.Wish.WishList.User.Auth0Id == userId).ToListAsync();

            //Get All images files 
            var dirPath = $"{options.BaseDirectory}\\{userId}";
            var unassignedFiles = Directory.GetFiles(dirPath).Where(f => !imagesInDB.Exists(i => i.Path == f)).ToList();

            foreach (var file in unassignedFiles)
            {
                //Debug.WriteLine(file);
                File.Delete(file);
            }

            return true;
        }

        /// <summary>
        /// Gets bytes array of stored image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns>Byte array of image</returns>
        /// <exception cref="FileServiceException">Thrown when could not get image from filesystem.</exception>
        public async Task<byte[]> GetStoredImageAsync(string imagePath)
        {
            try
            {
                Guard.IsNullEmptyOrWhiteSpace(imagePath, nameof(imagePath));

                if (!File.Exists(imagePath)) throw new FileNotFoundException();

                return await File.ReadAllBytesAsync(imagePath);
            }
            catch (Exception ex)
            {
                throw new FileServiceException("Could not get image from filesystem", ex);
            }
        }

        /// <summary>
        /// Delete image from filesystem for given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to file to be deleted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null, empty, or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when file could not be found or path is invalid</exception>
        public void Delete(string path)
        {
            Guard.IsNullEmptyOrWhiteSpace(path, nameof(path));
            Guard.IsValidPath(path);
            File.Delete(path);
        }

        /// <summary>
        /// Deletes wishlist folder with images. 
        /// </summary>
        /// <param name="id">Id/folder name of wishlist.</param>
        /// <param name="userId">Id/Folder name of user.</param>
        public void DeleteWishlistStore(string wishlistId, string userId)
        {
            Guard.IsNullEmptyOrWhiteSpace(wishlistId);
            Guard.IsNullEmptyOrWhiteSpace(userId);

            var pathToDelete = $"{options.BaseDirectory}\\{userId}\\{wishlistId}";

            if (Directory.Exists(pathToDelete)) Directory.Delete(pathToDelete, true);
        }
    }
}
