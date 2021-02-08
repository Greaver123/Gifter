using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess.Models;
using Gifter.Services.DTOs.Image;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class UploadServiceTests: TestWithSQLiteBase
    {
        private readonly UploadService uploadService;
        private string ImageSourcePath;

        public UploadServiceTests()
        {
            var filesService = new FilesService(DbContext, StoreOptions);
            var imageService = new ImageService(DbContext, StoreOptions);
            uploadService = new UploadService(DbContext, StoreOptions, imageService, filesService);

            var user = new User()
            {
                Auth0Id = "vcxvsdfsdfs",
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
            ImageSourcePath = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";
        }

        [TestMethod]
        public async Task UploadImage_WishExistsAndImageNotUploaded_Success()
        {
            var user = DbContext.Users.FirstOrDefault();
            var wishlist = DbContext.Wishlists.FirstOrDefault(w => w.UserId == user.Id);

            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await uploadService.UploadAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, user.Auth0Id);

                Assert.AreEqual(true, result);
            }
        }

        [TestMethod]
        public async Task UploadImage_TextFile_UploadFileException()
        {
            var user = DbContext.Users.FirstOrDefault();
            var wishlist = DbContext.Wishlists.FirstOrDefault(w => w.UserId == user.Id);

            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<UploadFileException>(async () =>
                {
                    var result = await uploadService.UploadAsync(new UploadImageDTO() { WishId = 1, ImageFile = formFile }, user.Auth0Id);
                });
            }
        }
    }
}
