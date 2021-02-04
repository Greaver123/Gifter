using Gifter.Common.Options;
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
    public class FileServiceTests
    {
        public string ImageSourcePath;
        public FilesService filesService;

        public FileServiceTests()
        {
            ImageSourcePath = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";
            filesService = new FilesService(
                Options.Create(
                    new StoreOptions()
                    {
                        BaseDirectory = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\ImagesDest",
                        UserStoreMaxSize = 100,
                        FileMaxSize = 5
                    }));
        }

        [TestMethod]
        public async Task SaveImageAsync_PngImage_FileCreatedAtUserDirectory()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await filesService.StoreImageAsync(formFile, "testDir");
                Assert.IsTrue(File.Exists(result));
            }
        }

        [TestMethod]
        public async Task SaveImageAsync_TxtFile_ThrowsArgumentException()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, "testDir");
                });
            }
        }

        [TestMethod]
        public async Task SaveImageAsync_File2Big_ThrowsArgumentException()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image20MB.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, "testDir");
                });
            }
        }

    }
}
