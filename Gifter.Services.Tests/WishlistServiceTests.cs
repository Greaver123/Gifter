using Gifter.Common.Exceptions;
using Gifter.DataAccess.Models;
using Gifter.Services.Constants;
using Gifter.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class WishlistServiceTests : TestWithSQLiteBase
    {
        WishlistService wishlistService;

        public string UserAuthId { get; set; } = "userAuthId3";

        public WishlistServiceTests() : base()
        {
            wishlistService = new WishlistService(DbContext, new FilesService(DbContext, StoreOptions));
            var user = new User()
            {
                Auth0Id = UserAuthId,
                Auth0Email = "test@gmail.com",
                Auth0Username = "JohnDoe",
            };

            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        [TestMethod]
        public async Task CreateWishlist_ValidParameters_ReturnsOperationResultSuccess()
        {
            var operationResult = await wishlistService.CreateWishlist("TEST", UserAuthId);
            var userPath = $"{ImageDestPath}\\{UserAuthId}";
            Assert.AreEqual(OperationStatus.SUCCESS, operationResult.Status);
            Assert.IsTrue(Directory.Exists($"{userPath}\\{operationResult.Data.Id}"));
            Directory.Delete(userPath, true);
        }

        [TestMethod]
        public async Task CreateWishlist_UserDoesNotExist_ReturnOperationFails()
        {
            var operationResult = await wishlistService.CreateWishlist("TEST", "fsdfs....");
            Assert.AreEqual(OperationStatus.ERROR, operationResult.Status);
            Assert.AreEqual("Internal error: Could not retrive user info.", operationResult.Message);
        }


        [TestMethod]
        public async Task CreateWishlist_ExceptionInFileService_ReturnOperationFails()
        {
            var fileServiceMock = new Mock<IFilesService>();
            var exceptionMessage = "Directory name is not valid.";
            fileServiceMock
                .Setup(fileService => fileService.CreateDirectoryForWishlist("1", UserAuthId))
                .Throws(new FileServiceException(exceptionMessage, new ArgumentException(exceptionMessage)));

            wishlistService = new WishlistService(DbContext, fileServiceMock.Object);
            
            var operationResult = await wishlistService.CreateWishlist("TEST", UserAuthId);
            var userPath = $"{ImageDestPath}\\{UserAuthId}";
            
            Assert.AreEqual(OperationStatus.ERROR, operationResult.Status);
            Assert.AreEqual("Internal error: Could not save Wishlist to database.", operationResult.Message);
            Assert.IsNull(DbContext.Wishlists.FirstOrDefault());
        }
    }
}
