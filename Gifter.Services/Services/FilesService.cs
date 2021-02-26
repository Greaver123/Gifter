using Gifter.Common;
using Gifter.Common.Exceptions;
using Gifter.Common.Extensions;
using Gifter.Common.Options;
using Gifter.DataAccess;
using Gifter.Services.Constants;
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
        /// Create wishlist directory for given user.
        /// </summary>
        /// <param name="userId">If of user.</param>
        /// <exception cref="ArgumentNullException">Thrown when path is null, empty or whitespace.</exception>
        /// <exception cref="ArgumentException">One one parameters has invalid characters for creating directory.</exception>
        /// <returns>Name of wishlist directory.</returns>
        public string CreateDirectoryForWishlist(string userId)
        {
            Guard.IsValidDirName(userId);

            try
            {
                var dirName = GetUniqueWishlistDirName(userId);
                var newWishlistDirectoryPath = GetWishlistDirectoryPath(userId, dirName);
                Directory.CreateDirectory(newWishlistDirectoryPath);

                return dirName;
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ExceptionMessages.FILESERVICE_CREATE_STORE_FAIL, ex);
            }
        }

        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object.</param>
        /// <param name="userId">Name of directory to store images.</param>
        /// <returns>FileName with exension.</returns>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters in null, empty or whitespace.</exception>
        /// <exception cref="FileServiceException">Thrown when could not store image due internal error.</exception>
        public async Task<string> StoreImageAsync(IFormFile formFile, string userId, string wishlistDirName)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNullEmptyOrWhiteSpace(wishlistDirName, nameof(wishlistDirName));
            Guard.IsNull(formFile);

            try
            {
                if (!formFile.IsImage()) throw new ArgumentException(ExceptionMessages.FILESERVICE_NOT_IMAGE);
                if (formFile.Length > options.FileMaxSize) throw new FormatException(ExceptionMessages.FILESERVICE_FILE_TOO_BIG);

                var fileName = CreateUniqueFileName(userId, wishlistDirName);
                var wishlistDir = GetWishlistDirectoryPath(userId, wishlistDirName);
                var extension = formFile.TryGetImageExtension();

                if (extension == null) throw new FormatException(ExceptionMessages.FILESERVICE_INVALID_SIGNATURE);

                var fileNameWithExtension = $"{fileName}{extension}";
                var fileFullpath = $"{wishlistDir}\\{fileNameWithExtension}";

                if (!Directory.Exists(wishlistDir)) throw new IOException(ExceptionMessages.FILESERVICE_DIR_NOT_EXIST);
                if (File.Exists(fileFullpath)) throw new IOException(ExceptionMessages.FILESERVICE_FILE_EXISTS);

                using (var stream = File.Create(fileFullpath))
                {
                    await formFile.CopyToAsync(stream);
                }
                return fileNameWithExtension;
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ExceptionMessages.FILESERVICE_SAVE_FAIL, ex);
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
        /// <exception cref="ArgumentNullException">Thrown when one of parameters in null, empty or whitespace.</exception>
        /// <exception cref="FileServiceException">Thrown when could not get image from filesystem.</exception>
        public async Task<byte[]> GetStoredImageAsync(string userId, string wishlitId, string fileName)
        {
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNullEmptyOrWhiteSpace(wishlitId, nameof(wishlitId));
            Guard.IsNullEmptyOrWhiteSpace(fileName, nameof(fileName));

            try
            {
                var imagePath = GetImagePath(userId, wishlitId, fileName);

                if (!File.Exists(imagePath)) throw new FileNotFoundException();

                return await File.ReadAllBytesAsync(imagePath);
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ExceptionMessages.FILESERVICE_GET_FAIL, ex);
            }
        }

        /// <summary>
        /// Delete image from filesystem for given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to file to be deleted.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters in null, empty or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when file could not be found or path is invalid.</exception>
        public void DeleteImage(string userId, string wishlistDirName, string fileName)
        {
            Guard.IsNullEmptyOrWhiteSpace(wishlistDirName, nameof(wishlistDirName));
            Guard.IsNullEmptyOrWhiteSpace(userId, nameof(userId));
            Guard.IsNullEmptyOrWhiteSpace(fileName, nameof(fileName));

            try
            {
                var imagePath = GetImagePath(userId, wishlistDirName, fileName);
                if (File.Exists(imagePath)) File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ExceptionMessages.FILESERVICE_DELETE_IMAGE_FAIL, ex);
            }
        }

        /// <summary>
        /// Deletes wishlist folder with images. 
        /// </summary>
        /// <param name="wishlistDirName">Directory name of wishlist.</param>
        /// <param name="userId">Id of user.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters is null, empty or whitespace.</exception>
        /// <exception cref="FileServiceException">Thrown when could not delete image due some internal exception.</exception>
        public void DeleteWishlistDirectory(string userId, string wishlistDirName)
        {
            Guard.IsNullEmptyOrWhiteSpace(wishlistDirName);
            Guard.IsNullEmptyOrWhiteSpace(userId);

            try
            {
                var pathToDelete = GetWishlistDirectoryPath(userId, wishlistDirName);

                if (Directory.Exists(pathToDelete)) Directory.Delete(pathToDelete, true);
            }
            catch (Exception ex)
            {
                throw new FileServiceException(ExceptionMessages.FILESERVICE_DELETE_STORE_FAIL, ex);
            }
        }

        private string GetWishlistDirectoryPath(string userId, string wishlistDirName)
        {
            return $"{options.BaseDirectory}\\{userId}\\{wishlistDirName}";
        }

        private string GetImagePath(string userId, string wishlistDirName, string fileName)
        {
            return $"{GetWishlistDirectoryPath(userId, wishlistDirName)}\\{fileName}";
        }

        private string GetUniqueWishlistDirName(string userId)
        {
            for (int i = 0; i < 5; i++)
            {
                var dirName = DateTime.Now.Ticks.ToString();
                var dirFullPath = GetWishlistDirectoryPath(userId, dirName);

                if (!Directory.Exists(dirFullPath)) return dirName;
            }

            throw new IOException(ExceptionMessages.FILESERVICE_CREATE_UNIQUE_DIR);
        }

        private string CreateUniqueFileName(string userId, string wishlistDirName)
        {
            for (int i = 0; i < 5; i++)
            {
                var fileName = DateTime.Now.Ticks.ToString();
                var dirFullPath = GetWishlistDirectoryPath(userId, wishlistDirName);

                if (!File.Exists($"{dirFullPath}//{fileName}")) return fileName;

            }

            throw new IOException(ExceptionMessages.FILESERVICE_CREATE_UNIQUE_FILENAME);
        }
    }
}
