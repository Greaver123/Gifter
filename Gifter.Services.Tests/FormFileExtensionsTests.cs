using Gifter.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class FormFileExtensionsTests
    {
        public string ImageSourcePath = @"C:\Users\pkolo\Repos\Gifter\Gifter.Services.Tests\Images\";

        [TestMethod]
        public void IsPng_PngFile_ReturnsTrue()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/png" };

                var result = formFile.IsPNG();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void IsJPG_JPGFile_ReturnsTrue()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.jpg"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };

                var result = formFile.IsJPG();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void GetExtension_PngFile_ReturnsExtensionString()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.png"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/png" };

                var result = formFile.TryGetImageExtension();

                Assert.AreEqual(".png", result);
            }
        }

        [TestMethod]
        public void GetExtension_JpgFile_ReturnsExtensionString()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.jpg"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };

                var result = formFile.TryGetImageExtension();

                Assert.AreEqual(".jpg", result);
            }
        }

        [TestMethod]
        public void GetExtension_JpegFile_ReturnsExtensionStringJPG()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.jpeg"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };

                var result = formFile.TryGetImageExtension();

                //JPG and JPEG The two terms have the same meaning and are interchangeable.
                Assert.AreEqual(".jpg", result);
            }
        }

        [TestMethod]
        public void GetExtension_GifFile_ReturnsExtensionString()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.gif"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/gif" };

                var result = formFile.TryGetImageExtension();

                Assert.AreEqual(".gif", result);
            }
        }

        [TestMethod]
        public void GetExtension_UnknownTypeTXT_ReturnsNull()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\image.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };

                var result = formFile.TryGetImageExtension();

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetExtension_FileSizeLessThan8Bytes_ReturnsNull()
        {
            using (var stream = File.OpenRead($"{ImageSourcePath}\\file4Bytes.txt"))
            {
                var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)) { Headers = new HeaderDictionary(), ContentType = "image/jpeg" };

                var result = formFile.TryGetImageExtension();

                Assert.IsNull(result);
            }
        }
    }
}
