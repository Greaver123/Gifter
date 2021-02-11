using Gifter.Common.Exceptions;
using Gifter.Common.Options;
using Gifter.DataAccess.Models;
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
    public class FileServiceTests : GifterDBContextTests
    {
        public const string IMAGESOURCEPATH = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";
        public const string BASEDESTPATH = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\ImagesDest";
        public const string USERAUTHID = "userAuthId2";

        public string FullUserDirPath
        {
            get
            {
                return $"{BASEDESTPATH}\\{USERAUTHID}";
            }
        }

        public FilesService filesService;

        public FileServiceTests() : base()
        {
            filesService = new FilesService(DbContext,
                Options.Create(
                    new StoreOptions()
                    {
                        BaseDirectory = BASEDESTPATH,
                        UserStoreMaxSize = 100,
                        FileMaxSize = 5000000, //5MB
                    }));

            var user = new User()
            {
                Auth0Id = USERAUTHID,
                Auth0Email = "test@gmail.com",
                Auth0Username = "JohnDoe",
                WishLists = new List<WishList> {
                    new WishList() {
                        Name = "Test Wishlist",
                        Wishes = new List<Wish>()
                        {
                            new Wish(){Name="Wish 1",Price=100, URL="URL 1"},
                            new Wish(){Name="Wish 2",Price=200, URL="URL 2"},
                            new Wish(){Name="Wish 3",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 4",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 5",Price=300, URL="URL 3"},
                            new Wish(){Name="Wish 6",Price=300, URL="URL 3"},
                        }
                    },
                }
            };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        [TestMethod]
        public async Task StoreImageAsync_PngImage_FileCreatedAtUserDirectory()
        {
            using (var stream = File.OpenRead($"{IMAGESOURCEPATH}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var result = await filesService.StoreImageAsync(formFile, "testDir");
                Assert.IsTrue(File.Exists(result));
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_TxtFile_ThrowsFormatException()
        {
            using (var stream = File.OpenRead($"{IMAGESOURCEPATH}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FormatException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, "testDir");
                });
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_InvalidCharactersInDirName_ThrowsIOException()
        {
            using (var stream = File.OpenRead($"{IMAGESOURCEPATH}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<IOException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, "auth|fdsfsdf");
                });
            }
        }

        [TestMethod]
        public async Task StoreImageAsync_File2Big_ThrowsArgumentException()
        {
            using (var stream = File.OpenRead($"{IMAGESOURCEPATH}\\image20MB.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                await Assert.ThrowsExceptionAsync<FileSizeException>(async () =>
                {
                    var result = await filesService.StoreImageAsync(formFile, "testDir");
                });
            }
        }

        [TestMethod]
        public async Task DeleteUnassignedImages_FewFiles_ReturnsTrue()
        {
            using (var stream = File.OpenRead($"{IMAGESOURCEPATH}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };
                var filesCount = 6;
                var filesPath = await CreateTestFilesFromStream(stream, filesCount);

                //Create entries with files in Image entity
                for (int i = 0; i < (filesCount / 2); i++)
                {
                    DbContext.Images.Add(new Image() { Path = filesPath[i], WishId = i + 1 });
                }

                await DbContext.SaveChangesAsync();
               
                Assert.AreEqual(3, DbContext.Images.Select(i => i).Count());
                var result = await filesService.DeleteUnassignedImages(USERAUTHID);
                
                Assert.IsTrue(result);
                Assert.AreEqual(3, Directory.GetFiles(FullUserDirPath).Count());

                var expected = new string[3];
                Array.Copy(filesPath, expected,3);

                //Check if delted correct files
                Assert.AreEqual(3, Directory.GetFiles(FullUserDirPath).Where(file => expected.Contains(file)).Count(),"Deleted wrong files");
            }
        }

        private async Task<string[]> CreateTestFilesFromStream(Stream stream, int count)
        {
            Directory.CreateDirectory(FullUserDirPath);
            var filesPaths = new string[count];

            //Create files with entires in Image entity
            for (int i = 0; i < count; i++)
            {
                var imageName = $"image{i}.png";
                var fullPath = $"{FullUserDirPath}\\{imageName}";

                using (var fileStream = File.Create(fullPath))
                {
                    await stream.CopyToAsync(fileStream);

                }

                stream.Position = 0;
                filesPaths[i] = fullPath;
            }

            Assert.IsTrue(Directory.GetFiles(FullUserDirPath).Length == count);
            Assert.IsTrue(filesPaths.Length == count);

            return filesPaths;
        }

        //[TestCleanup]
        //public void CleanTestDir()
        //{
        //    Directory.Delete(FullUserDirPath, true);
        //    Assert.IsTrue(!Directory.Exists(FullUserDirPath));
        //}
    }
}
