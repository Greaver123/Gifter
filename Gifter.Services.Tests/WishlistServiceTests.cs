using Gifter.Common.Exceptions;
using Gifter.DataAccess.Models;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.DTOS.Wishlist;
using Gifter.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class WishlistServiceTests : TestWithSQLiteBase
    {
        WishlistService wishlistService;


        public WishlistServiceTests() : base()
        {
            wishlistService = new WishlistService(DbContext, new FilesService(DbContext, StoreOptions));
        }

        [TestMethod]
        public async Task BulkEditWishlist_WishlistExistAddWish_ReturnsSuccess()
        {
            //Arrange
            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "fdsfsdfsdfsvcx"
            };
            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            //Act

            var operationResult = await wishlistService
                .BulkEditWishlist(
                new WishlistEditDTO()
                {
                    Id = wishlist.Id,
                    Wishes = new List<WishCreateDTO>()
                    {
                        new WishCreateDTO()
                        {
                            Name = "Wish 1",
                            Price = 1000
                        }
                    }
                }, UserId);

            //Assert
            Assert.AreEqual(OperationStatus.SUCCESS, operationResult.Status);
            Assert.IsNotNull(operationResult.Data);
        }

        [TestMethod]
        public async Task BulkEditWishlist_WishlistDoesNotExistAddWish_ReturnsFail()
        {
            //Arrange
            var wishlistEdit = new WishlistEditDTO()
            {
                Id = 12345,
                Wishes = new List<WishCreateDTO>()
                    {
                        new WishCreateDTO()
                        {
                            Name = "Wish 1",
                            Price = 1000
                        }
                    }
            };

            //Act
            var operationResult = await wishlistService.BulkEditWishlist(wishlistEdit, UserId);


            //Assert
            Assert.AreEqual(OperationStatus.FAIL, operationResult.Status);
            Assert.IsNull(operationResult.Data);
        }

        [TestMethod]
        public async Task CreateWishlist_ValidParameters_ReturnsOperationResultSuccess()
        {
            var operationResult = await wishlistService.CreateWishlist("TEST", UserId);
            Assert.AreEqual(OperationStatus.SUCCESS, operationResult.Status);
        }

        [TestMethod]
        public async Task CreateWishlist_UserDoesNotExist_ReturnOperationFails()
        {
            var operationResult = await wishlistService.CreateWishlist("TEST", "fsdfs....");
            Assert.AreEqual(OperationStatus.ERROR, operationResult.Status);
        }

        [TestMethod]
        public async Task CreateWishlist_ExceptionInFileService_ReturnOperationFails()
        {
            //Arrange
            var fileServiceMock = new Mock<IFilesService>();
            var exceptionMessage = "Directory name is not valid.";
            fileServiceMock
                .Setup(fileService => fileService.CreateDirectoryForWishlist("v...."))
                .Throws(new FileServiceException(exceptionMessage, new ArgumentException(exceptionMessage)));

            wishlistService = new WishlistService(DbContext, fileServiceMock.Object);


            //Act
            var operationResult = await wishlistService.CreateWishlist("TEST", UserId);

            //Assert
            Assert.AreEqual(OperationStatus.ERROR, operationResult.Status);
        }

        [TestMethod]
        public async Task DeleteWishlist_WishlistWithWish_ReturnOperationSuccess()
        {
            //Arrange
            var wishlistDirName = "fdsfsdfssf";
            var imageFileName = "fdsfdsfsd.png";

            var wishlist = new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = wishlistDirName,
                Wishes = new List<Wish>(){
                    new Wish(){
                        Name = "Wish 1"
                        ,Image = new Image()
                        {
                            FileName = imageFileName
                        }
                    }
                }
            };

            DbContext.Wishlists.Add(wishlist);
            DbContext.SaveChanges();

            var wishlistDirPath = $"{UserDirectory}\\{wishlistDirName}";

            Directory.CreateDirectory(wishlistDirPath);
            File.Copy($"{ImageSrcPath}\\image.png",$"{wishlistDirPath}\\{imageFileName}");

            //Act
            await wishlistService.DeleteWishlist(1, UserId);

            //Assert
            Assert.AreEqual(0, DbContext.Wishlists.Count());
            Assert.IsFalse(Directory.Exists(wishlistDirPath));
        }
    }
}
