using Gifter.Common.Exceptions;
using Gifter.DataAccess.Models;
using Gifter.Services.Common;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Image;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class ImageServiceTests : TestWithSQLiteBase
    {
        private readonly ImageService uploadService;

        public ImageServiceTests() : base()
        {
            var filesService = new FilesService(DbContext, StoreOptions);
            uploadService = new ImageService(DbContext, StoreOptions, filesService);
        }

        [TestMethod]
        public async Task GetImage_WishExists_ReturnsOperationResultSuccess()
        {
            //Arrange
            var wishlistDirName = "fdsfdsfsdf";
            var fileName = "fdsfdsfsd.png";
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1",
                        Image = new Image()
                        {
                            FileName =fileName,
                        }
                    }
                }
                ,
                DirectoryName = wishlistDirName
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");

            var fileDestPath = $"{UserDirectory}\\{wishlistDirName}\\{fileName}";
            File.Copy($"{ImageSrcPath}\\image.png", fileDestPath);

            //Act
            var actualResult = await uploadService.GetImageAsync(1, UserId);

            //Assert
            Assert.IsNotNull(actualResult.Data.Image);
            Assert.AreEqual(OperationStatus.SUCCESS, actualResult.Status);

            //Cleanup
            Directory.Delete($"{UserDirectory}\\{wishlistDirName}", true);
        }

        [TestMethod]
        public async Task GetImage_WishDoesNotExists_ReturnsOperationResultFail()
        {
            //Arrange
            var wishlistDirName = "fdsfdsfsdf";
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = wishlistDirName
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");

            //Act
            var actualResult = await uploadService.GetImageAsync(1, UserId);

            //Assert
            Assert.IsNull(actualResult.Data);
            Assert.AreEqual(OperationStatus.FAIL, actualResult.Status);

            //Cleanup
            Directory.Delete($"{UserDirectory}\\{wishlistDirName}", true);
        }

        [TestMethod]
        public async Task DeleteImage_WishExistsAndImageDoesNotExist_Fail()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1",
                    }
                }
                ,DirectoryName = "bvcbcxbcv",
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var operationResult = await uploadService.DeleteImageAsync(131231231, UserId);

            //Assert
            Assert.AreEqual(OperationStatus.FAIL, operationResult.Status);
        }

        [TestMethod]
        public async Task DeleteImage_WishExistsAndImageExists_Success()
        {
            //Arrange
            var wishlistDirName = "fdsfdsfsdf";
            var fileName = "fdsfdsfsd.png";

            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1",
                        Image = new Image()
                        {
                            FileName =fileName,
                        }
                    }
                }
                ,
                DirectoryName = wishlistDirName
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");

            var fileDestPath = $"{UserDirectory}\\{wishlistDirName}\\{fileName}";
            File.Copy($"{ImageSrcPath}\\image.png", fileDestPath);

            //Act
            var operationResult = await uploadService.DeleteImageAsync(1, UserId);

            //Assert
            Assert.AreEqual(0, DbContext.Images.Count());
            Assert.AreEqual(OperationStatus.SUCCESS, operationResult.Status);
        }

        [TestMethod]
        public async Task UploadImage_WishExistsAndImageNotUploaded_Success()
        {
            //Arrange
            var wishlistDirName = "fdsfsdfssf";
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = wishlistDirName,
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1"
                    }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");

            //Act
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadImageAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                //Assert
                Assert.AreEqual(OperationStatus.SUCCESS, result.Status);
            }

            //Cleanup
            Directory.Delete($"{UserDirectory}\\{wishlistDirName}",true);
        }

        [TestMethod]
        public async Task UploadImage_WishDoesNotExist_ReturnsFails()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "fdsfdsfsdf"

            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadImageAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                //Assert
                Assert.AreEqual(OperationStatus.FAIL, result.Status);
            }
        }

        [TestMethod]
        public async Task UploadImage_TextFile_ReturnsOperationResultError()
        {
            //Arrange
            var wishlistDirName = "fdsfsdfsdvxc";
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = wishlistDirName,
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1"
                    }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            using (var stream = File.OpenRead($"{ImageSrcPath}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadImageAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                //Assert
                Assert.AreEqual(OperationStatus.ERROR, result.Status);
            }
        }

        [TestMethod]
        public async Task DownloadImage_WishExists_ReturnsOperationResultSuccess()
        {
            //Arrange
            var wishlistDirName = "fdsfdsfsdf";
            var fileName = "fdsfdsfsd.png";
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1",
                        Image = new Image()
                        {
                            FileName =fileName,
                        }
                    }
                }
                ,
                DirectoryName = wishlistDirName
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");

            var fileDestPath = $"{UserDirectory}\\{wishlistDirName}\\{fileName}";
            File.Copy($"{ImageSrcPath}\\image.png", fileDestPath);

            //Act
            var actualResult = await uploadService.DownloadImageAsync(1, UserId);

            //Assert
            Assert.IsNotNull(actualResult.Data.Image);
            Assert.AreEqual(OperationStatus.SUCCESS, actualResult.Status);

            //Cleanup
            Directory.Delete($"{UserDirectory}\\{wishlistDirName}",true);
        }

        [TestMethod]
        public async Task DownloadImage_WishDoesNotExist_ReturnsOperationResultFail()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "fdsfsdfsdfvcx",
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(9999, UserId);

            //Assert
            Assert.IsNull(actualResult.Data);
            Assert.AreEqual(OperationStatus.FAIL, actualResult.Status);
        }

        [TestMethod]
        public async Task DownloadImage_ImageDontExistOnFileSystem_ReturnsOperationResultError()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "bvcbcvbsf",
                Wishes = new List<Wish>()
                {
                    new Wish()
                    {
                        Name = "Test",
                        Price = 9999,
                        Image = new Image()
                        {
                            FileName = "fdsfdsfdsfds.png"
                        },
                    }
                }
            };

            DbContext.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(1, UserId);

            //Assert
            Assert.IsNull(actualResult.Data);
            Assert.AreEqual(OperationStatus.ERROR, actualResult.Status);
        }

        [TestMethod]
        public async Task DownloadImage_NoFileExtension_ReturnsOperationResultError()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName= "fdsfsdfsdfsd",
                Wishes = new List<Wish>(){
               new Wish()
                 {
                     Name = "Test",
                     Price = 9999,
                     Image = new Image()
                     {
                         FileName="gfdgdfgdf"
                     },
                 }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(1, UserId);

            //Assert
            Assert.IsNull(actualResult.Data);
            Assert.AreEqual(OperationStatus.ERROR, actualResult.Status);
        }
    }
}
