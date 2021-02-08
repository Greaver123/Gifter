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
using Gifter.Common.Extensions;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class ImageServiceTests : TestWithSQLiteBase
    {
        ImageService imageService;

        public string ImageSourcePath;

        public ImageServiceTests() : base()
        {
            imageService = new ImageService(DbContext, StoreOptions);
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
        public void CheckIfUserExists()
        {
            var user = DbContext.Users.FirstOrDefault();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task AddImage_ValidParameters_ReturnsImageId()
        {
            //TODO 
            //var user = DbContext.Users.FirstOrDefault();
            //var wishlist = DbContext.Wishlists.FirstOrDefault(w => w.UserId == user.Id);
            //await imageService.AddImage(1, "path", user.Auth0Id);
        }
    }
}
