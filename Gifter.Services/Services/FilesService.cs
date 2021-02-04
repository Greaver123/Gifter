using Gifter.Common.Extensions;
using Gifter.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public class FilesService : IFilesService
    {
        private readonly StoreOptions options;

        public FilesService(IOptions<StoreOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// Creates image in users directory with random unique name.
        /// </summary>
        /// <param name="formFile">FormFile object</param>
        /// <param name="dirName">Name of directory to store images</param>
        /// <returns>Full path to created image.</returns>
        /// <exception cref="IOException">Thrown when file already exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when formFile or dirName is null/empty or whitespace</exception>
        /// <exception cref="ArgumentException">Thrown when formFile is not a image.</exception>
        public async Task<string> StoreImageAsync(IFormFile formFile, string dirName)
        {
            if (formFile == null) throw new ArgumentNullException(nameof(formFile));
            if (string.IsNullOrWhiteSpace(dirName)) throw new ArgumentNullException($"{dirName} cannot be null, empty or whitespace.");
            if (!formFile.IsImage()) throw new ArgumentException("File is not a image.");
            if (formFile.Length > options.FileMaxSize * 1000000) throw new ArgumentException("File is too big.");

            //var name = $"{Path.GetRandomFileName().Replace(".", "")}{DateTime.Now.Ticks}";
            var name = $"{DateTime.Now.Ticks}";
            var userDir = $"{this.options.BaseDirectory}\\{dirName}";
            var extension = formFile.TryGetImageExtension();

            if (extension == null) throw new ArgumentException("File signature not recognised.");

            var fileFullpath = $"{userDir}\\{name}{extension}";

            if (File.Exists(fileFullpath)) throw new IOException($"File already exists with given name: {name}.");

            Directory.CreateDirectory(userDir);

            using (var stream = File.Create(fileFullpath))
            {
                await formFile.CopyToAsync(stream);
            }

            return fileFullpath;
        }
    }
}
