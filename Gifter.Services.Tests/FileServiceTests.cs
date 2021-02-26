using Gifter.Common;
using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess.Models;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class FileServiceTests : GifterDBContextTests
    {
        public FilesService filesService;

        public FileServiceTests() : base()
        {
            filesService = new FilesService(DbContext,
                Options.Create(
                    new StoreOptions()
                    {
                        BaseDirectory = ImageDestPath,
                        UserStoreMaxSize = 100,
                        FileMaxSize = 5000000, //5MB
                    }));

            var user = new User()
            {
                Auth0Id = UserId,
                Auth0Email = "test@gmail.com",
                Auth0Username = "JohnDoe",
                WishLists = new List<WishList> {
                    new WishList() {
                        Name = "Test Wishlist",
                        DirectoryName = "dsadasdasd",
                        Wishes = new List<Wish>()
                        {
                            new Wish(){Name="Wish 1",Price=100, URL="URL 1"},
                            new Wish(){Name="Wish 2",Price=200, URL="URL 2"},
                            new Wish(){Name="Wish 3",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 4",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 5",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 6",Price=300, URL="URL 3"},
                        }
                    },
                }
            };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        [TestMethod]
        public async Task StoreImageAsync_PngImage_FileCreatedAtUserDirectory()
        {
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
            {
                var wishlistDirName = "fjlskdjflskdfjfjlds";
                var wishlistPath = $"{UserDirectory}\\{wishlistDirName}";
                Directory.CreateDirectory(wishlistPath);
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await filesService.StoreImageAsync(formFile, UserId, wishlistDirName);
                Assert.IsNotNull(result);
                Assert.IsTrue(Directory.GetFiles(wishlistPath).Length > 0);
                Directory.Delete(wishlistPath, true);
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_WishlistDirDoesNotExist_ThrowsFileServiceException()
        {
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
            {
                var wishlistDirName = "fjlskdjflskdfjfjlds";
                var wishlistPath = $"{UserDirectory}\\{wishlistDirName}";
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FileServiceException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, UserId, wishlistDirName);

                });
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_TxtFile_ThrowsFileServiceException()
        {
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.txt"))
            {
                var wishlistDirName = "fjlskdjflskdfjfjlds";
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FileServiceException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, UserId, wishlistDirName);
                });
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_InvalidCharactersInDirName_ThrowsFileServiceException()
        {
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
            {
                var wishlistDirName = "fjlskdjflskdfjfjlds......";
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FileServiceException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, UserId, wishlistDirName);
                });
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_File2Big_ThrowsFileServiceException()
        {
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image20MB.png"))
            {
                var wishlistDirName = "fjlskdjflskdfjfjlds";
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FileServiceException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, UserId, wishlistDirName);
                });
            }
        }

        //[TestMethod]
        //public async Task DeleteUnassignedImages_FewFiles_ReturnsTrue()
        //{
        //    using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
        //    {
        //        var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
        //        var filesCount = 6;
        //        var filesPath = await CreateTestFilesFromStream(stream, UserDirectory, filesCount);

        //        //Create entries with files in Image entity
        //        for (int i = 0; i < (filesCount / 2); i++)
        //        {
        //            DbContext.Images.Add(new Image() { Path = filesPath[i], WishId = i + 1 });
        //        }

        //        await DbContext.SaveChangesAsync();

        //        Assert.AreEqual(3, DbContext.Images.Select(i => i).Count());
        //        var result = await filesService.DeleteUnassignedImages(UserId);

        //        Assert.IsTrue(result);
        //        Assert.AreEqual(3, Directory.GetFiles(UserDirectory).Count());

        //        var expected = new string[3];
        //        Array.Copy(filesPath, expected, 3);

        //        //Check if delted correct files
        //        Assert.AreEqual(3, Directory.GetFiles(UserDirectory).Where(file => expected.Contains(file)).Count(), "Deleted wrong files");
        //    }
        //}

        private async Task<string[]> CreateTestFilesFromStream(Stream stream, string path, int count)
        {
            Directory.CreateDirectory(path);
            var filesPaths = new string[count];

            //Create files with entires in Image entity
            for (int i = 0; i < count; i++)
            {
                var imageName = $"image{i}.png";
                var fullPath = $"{path}\\{imageName}";

                using (var fileStream = File.Create(fullPath))
                {
                    await stream.CopyToAsync(fileStream);

                }

                stream.Position = 0;
                filesPaths[i] = fullPath;
            }

            Assert.IsTrue(Directory.GetFiles(path).Length == count);
            Assert.IsTrue(filesPaths.Length == count);

            return filesPaths;
        }

        [TestMethod]
        public void CreateDirectoryForUser_PathDoesNotExists_ReturnsDirName()
        {
            //Arrange
            string userId = "bgdb1451tgfdg";

            //Act
            var dirName = filesService.CreateDirectoryForWishlist(userId);

            //Assert
            Assert.IsNotNull(dirName);
        }

        [TestMethod]
        public void CreateDirectoryForUser_InvalidCharactersInFolderName_ThrowsException()
        {
            string userId = "bgdb1451tgfdg...";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var fullPathActual = filesService.CreateDirectoryForWishlist(userId);
            });
        }

        [TestMethod]
        public void IsValidFolderName_ValidFolder()
        {
            Guard.IsValidDirName("aabbccdd");
            Guard.IsValidDirName("aabb ccdd");
            Guard.IsValidDirName("aabb     ccdd");
            Guard.IsValidDirName("aabb.ccdd");
            Guard.IsValidDirName("_aabb.ccdd");
            Guard.IsValidDirName("_aabb.ccdd..txt");
        }

        [TestMethod]
        public void IsValidFolderName_InvalidFolderName_ThrownsArgumentException()
        {
            var folderNames = new string[] {
                null,
                "",
                "       ",
                "       .",
                "       ..",
                "aabbccdd/",
                "aabbccdd\\",
                "aabbccdd:",
                "aabbccdd|",
                "aabbccdd*",
                "aabbccdd?",
                "aabbccdd\"",
                "aabbccdd.",
                "aabbccdd..",
                "./\\:|*?\"..",
                "aab/\\ *|?. .. bccdd.",
            };

            foreach (var name in folderNames)
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    Guard.IsValidDirName(name);

                }, $"Folder name: \"{name}\"");
            }
        }

        [TestMethod]
        public async Task DeleteWishlistStore_ExistingPath_DeleteSuccess()
        {
            var wishlistDirName = "fdsfsdfsdfsdf";
            var dir1 = $"{ImageDestPath}\\{UserId}\\{wishlistDirName}";
            var dir2 = $"{ImageDestPath}\\{UserId}\\{2}";

            Directory.CreateDirectory(dir1);
            Directory.CreateDirectory(dir2);

            await CreateTestFilesFromStream(File.OpenRead($"{ImageSrcPath}\\image.png"),dir1,10);
            await CreateTestFilesFromStream(File.OpenRead($"{ImageSrcPath}\\image.png"), dir2, 10);

            filesService.DeleteWishlistDirectory(UserId, wishlistDirName);

            Assert.IsFalse(Directory.Exists(dir1));
            Assert.IsTrue(Directory.Exists(dir2));

            Directory.Delete(dir2,true);
        }
    }
}
