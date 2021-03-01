using Gifter.DataAccess.Models;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.Services;
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
    public class WishServiceTests : TestWithSQLiteBase
    {
        private readonly WishService wishService;

        public WishServiceTests() : base()
        {
            wishService = new WishService(DbContext, new FilesService(DbContext, StoreOptions));
        }

        [TestMethod]
        public async Task DeleteWish_WishExistWithoutImage_ReturnTrue()
        {

            //Arrange
            DbContext.Wishlists.Add(new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "avavreghv",
                Wishes = new List<Wish>() {
                    new Wish() {
                        Name = "Wish 1",
                        Price = 12345,
                        URL = "www.wish1.com" } },
            });

            DbContext.SaveChanges();

            //act
            var result = await wishService.DeleteAsync(1, UserId);


            //Assert
            Assert.IsTrue(result.Data);
            Assert.AreEqual(0, DbContext.Wishes.Count());
        }

        [TestMethod]
        public async Task DeleteWish_WishExistWithImage_ReturnTrue()
        {
            //Arrange
            var fileName = "fdsfsdvcxs.png";
            var wishlistDirName = "avavreghv";
            var fullFilePath = $"{UserDirectory}\\{wishlistDirName}\\{fileName}";
           
            Directory.CreateDirectory($"{UserDirectory}\\{wishlistDirName}");
            File.Copy($"{ImageSrcPath}\\image.png", fullFilePath);

            DbContext.Wishlists.Add(new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = wishlistDirName,
                Wishes = new List<Wish>() {
                    new Wish() {
                        Name = "Wish 1",
                        Price = 12345,
                        URL = "www.wish1.com",
                    Image = new Image(){ 
                        FileName = fileName}, 
                    }
            }
            });

            DbContext.SaveChanges();

            //Act
            var result = await wishService.DeleteAsync(1, UserId);

            //Assert
            Assert.IsTrue(result.Data);
            Assert.AreEqual(0, DbContext.Wishes.Count());
            Assert.IsFalse(File.Exists(fullFilePath));

            //Cleanup 
            Directory.Delete($"{UserDirectory}\\{wishlistDirName}");
        }


        [TestMethod]
        public async Task AddWish_EmptyWishWithWishlistId_ReturnWishId()
        {
            //Arrange
            DbContext.Wishlists.Add(new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "avavreghv",
            });

            DbContext.SaveChanges();

            var addWish = new AddWishDTO()
            {
                WishlistId = 1,
            };

            //Act
            var operationResult = await wishService.AddAsync(addWish, UserId);

            //Assert
            Assert.AreEqual(1, operationResult.Data.Id);
        }

        [TestMethod]
        public async Task AddWish_Wish_ReturnWishId()
        {

            //Arrange
            DbContext.Wishlists.Add(new WishList()
            {
                UserId = 1,
                Name = "Wishlist 1",
                DirectoryName = "avavreghv",
            });

            DbContext.SaveChanges();

            var addWish = new AddWishDTO()
            {
                WishlistId = 1,
                Name = "Wish 1",
                Price = 88.99,
                URL = "www.wish1.com"
            };

            //Act
            var operationResult = await wishService.AddAsync(addWish, UserId);

            //Assert
            Assert.IsTrue(operationResult.Status == OperationStatus.SUCCESS);
            Assert.AreEqual(1, operationResult.Data.Id);
        }

        [TestMethod]
        public async Task AddWish_WishlistDoesNotExist_ReturnNull()
        {
            //Arrange
            var addWish = new AddWishDTO()
            {
                WishlistId = 12345,
                Name = "Wish 1",
                Price = 88.99,
                URL = "www.wish1.com"
            };

            //Act
            var operationResult = await wishService.AddAsync(addWish, UserId);

            //Assert
            Assert.IsNull(operationResult.Data);
        }
    }
}
