using Gifter.Controllers;
using Gifter.Services.DTOS.Image;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gifter.Tests
{
    [TestClass]
    public class ImagesControllerTests
    {
        [TestMethod]
        public async Task GetImage_WishIdExists_ReturnsOkResponse()
        {
            //int wishId = 1;

            //var mockUploadService = new Mock<IUploadService>();
            //mockUploadService
            //    .Setup(u => u.DownloadImageAsync(wishId, null))
            //    .ReturnsAsync(new DownloadImageDTO() { 
            //        FileExtension = "png", 
            //        Image = File.ReadAllBytes("C:\\Users\\pkolo\\Repos\\Gifter\\Gifter.Tests\\Images\\image.png") });

            //var controller = new ImageController(mockUploadService.Object);

            //var result = await controller.Get(wishId);

            //Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.Fail("Not implemented");
        }

        [TestMethod]
        public async Task GetImage_WishDoesntExist_ReturnsNotFoundResponse()
        {
            //int doesNotExistId =9999;

            //var mockUploadService = new Mock<IUploadService>();
            //mockUploadService
            //    .Setup(u => u.DownloadImageAsync(doesNotExistId, null)).ReturnsAsync((DownloadImageDTO)null);

            //var controller = new ImageController(mockUploadService.Object);
            //var result = await controller.Get(doesNotExistId);

            //Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.Fail("Not Implemented");
        }
    }
}
