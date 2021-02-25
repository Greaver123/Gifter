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
    public class UploadServiceTests : TestWithSQLiteBase
    {
        private readonly UploadService uploadService;
        private string ImageSourcePath;

        public UploadServiceTests() : base()
        {
            var filesService = new FilesService(DbContext, StoreOptions);
            var imageService = new ImageService(DbContext, StoreOptions);
            uploadService = new UploadService(DbContext, StoreOptions, imageService, filesService);
            ImageSourcePath = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";
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
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var operationResult = await uploadService.DeleteImageFromWish(131231231, UserId);

            //Assert
            Assert.AreEqual(OperationStatus.FAIL, operationResult.Status);
        }
        [TestMethod]
        public async Task DeleteImage_WishExistsAndImageExists_Success()
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
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlist.Id}");
            File.Copy($"{ImageSourcePath}\\image.png", $"{UserDirectory}\\{wishlist.Id}\\image.png");

            DbContext.Images.Add(new Image() { WishId = wishlist.Wishes.First().Id, Path = $"{UserDirectory}\\{wishlist.Id}\\image.png" });
            DbContext.SaveChanges();


            //Act
            var image = DbContext.Images.FirstOrDefault();
            var operationResult = await uploadService.DeleteImageFromWish(image.Id, UserId);

            //Assert
            Assert.AreEqual(0, DbContext.Images.Count());
            Assert.AreEqual(OperationStatus.SUCCESS, operationResult.Status);
        }

        [TestMethod]
        public async Task UploadImage_WishExistsAndImageNotUploaded_Success()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1"
                    }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlist.Id}");

            //Act
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                //Assert
                Assert.AreEqual(OperationStatus.SUCCESS, result.Status);
            }
        }

        [TestMethod]
        public async Task UploadImage_WishDoesNotExist_ReturnsFails()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",

            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlist.Id}");

            //Act
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                //Assert
                Assert.AreEqual(OperationStatus.FAIL, result.Status);
            }
        }

        [TestMethod]
        public async Task UploadImage_TextFile_ReturnsOperationResultError()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1"
                    }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();
            Directory.CreateDirectory($"{UserDirectory}\\{wishlist.Id}");

            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, UserId);

                Assert.AreEqual(OperationStatus.ERROR, result.Status);
            }
        }

        [TestMethod]
        public async Task DownloadImage_WishExists_ReturnsOperationResultSuccess()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                Wishes = new List<Wish>(){
               new Wish()
                 {
                     Name = "Test",
                     Price = 9999,
                     Image = new Image()
                     {
                         Path = $"{this.ImageSourcePath}\\image.png"
                     },
                 }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(wishlist.Wishes.First().Id, UserId);

            //Assert
            Assert.IsNotNull(actualResult.Data.Image);
            Assert.AreEqual(OperationStatus.SUCCESS, actualResult.Status);
        }

        [TestMethod]
        public async Task DownloadImage_WishDoesNotExist_ReturnsOperationResultFail()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
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
                Wishes = new List<Wish>()
                {
                    new Wish()
                    {
                        Name = "Test",
                        Price = 9999,
                        Image = new Image()
                        {
                            Path = $"{this.ImageSourcePath}\\dontExist.png"
                        },
                    }
                }
            };

            DbContext.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(wishlist.Wishes.First().Id, UserId);

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
                Wishes = new List<Wish>(){
               new Wish()
                 {
                     Name = "Test",
                     Price = 9999,
                     Image = new Image()
                     {
                         Path = $"{this.ImageSourcePath}\\image"
                     },
                 }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act
            var actualResult = await uploadService.DownloadImageAsync(wishlist.Wishes.First().Id, UserId);

            //Assert
            Assert.IsNull(actualResult.Data);
            Assert.AreEqual(OperationStatus.ERROR, actualResult.Status);
        }
    }
}
