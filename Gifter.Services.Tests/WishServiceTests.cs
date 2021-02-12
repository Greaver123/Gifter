using Gifter.DataAccess.Models;
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

        public string UserAuthId { get; set; } = "userAuthId3";

        public string UserDirPath
        {
            get
            {
                return $"{ImageDestPath}//{UserAuthId}";
            }
        }

        public WishServiceTests() : base()
        {
            wishService = new WishService(DbContext, new FilesService(DbContext, StoreOptions));
            var user = new User()
            {
                Auth0Id = UserAuthId,
                Auth0Email = "test@gmail.com",
                Auth0Username = "JohnDoe",
                WishLists = new List<WishList> {
                    new WishList() {
                        Name = "Test Wishlist",
                        Wishes = new List<Wish>()
                        {
                            new Wish(){Name="Wish 1",Price=100, URL="URL 1"},
                            new Wish(){Name="Wish 2",Price=200, URL="URL 2"},
                        }
                    },
                }
            };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        [TestMethod]
        public async Task DeleteWish_WishExistWithoutImage_ReturnTrue()
        {
            var result = await wishService.DeleteAsync(1, "userAuthId3");

            Assert.IsTrue(result);
            Assert.AreEqual(1, DbContext.Wishes.Count());
        }

        [TestMethod]
        public async Task DeleteWish_WishExistWithImage_ReturnTrue()
        {
            //Arrange
            var fullFilePath = $"{UserDirPath}\\image.png";

            Directory.CreateDirectory(UserDirPath);
            File.Copy($"{ImageSrcPath}\\image.png", fullFilePath);

            var wish = DbContext.Wishes.FirstOrDefault(w => w.Id == 1);
            wish.Image = new Image() { Path = fullFilePath };
            DbContext.SaveChanges();

            //Act
            var result = await wishService.DeleteAsync(1, UserAuthId);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, DbContext.Wishes.Count());
            Assert.IsFalse(File.Exists(fullFilePath));
        }


        [TestMethod]
        public async Task AddWish_EmptyWishWithWishlistId_ReturnWishId()
        {
            var addWish = new AddWishDTO()
            {
                WishlistId = 1,
            };

            //Act
            var createdWishId = await wishService.AddAsync(addWish, UserAuthId);

            //Assert
            Assert.AreEqual(3, createdWishId);
        }

        [TestMethod]
        public async Task AddWish_Wish_ReturnWishId()
        {
            var addWish = new AddWishDTO()
            {
                WishlistId = 1,
                Name = "Wish 1",
                Price = 88.99,
                URL = "www.wish1.com"
            };

            //Act
            var createdWishId = await wishService.AddAsync(addWish, UserAuthId);

            //Assert
            Assert.AreEqual(3, createdWishId);
        }

        [TestMethod]
        public async Task AddWish_WishlistDoesNotExist_ReturnNull()
        {
            var addWish = new AddWishDTO()
            {
                WishlistId = 12345,
                Name = "Wish 1",
                Price = 88.99,
                URL = "www.wish1.com"
            };

            //Act
            var createdWishId = await wishService.AddAsync(addWish, UserAuthId);

            //Assert
            Assert.IsNull(createdWishId);
        }
    }
}
