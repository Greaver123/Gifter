using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Services.Constants
{
    /// <summary>
    /// Messages for exceptions.
    /// </summary>
    public static class ExceptionMessages
    {
        //FileService Exception messages
        public const string FILESERVICE_SAVE_FAIL = "Could not store image.";
        public const string FILESERVICE_GET_FAIL = "Could not get image from filesystem.";
        public const string FILESERVICE_DELETE_IMAGE_FAIL = "Could not delete image.";
        public const string FILESERVICE_CREATE_STORE_FAIL = "Could not create wishlist store.";
        public const string FILESERVICE_DELETE_STORE_FAIL = "Could not delete wishlist store.";
        public const string FILESERVICE_NOT_IMAGE = "File is not a image.";
        public const string FILESERVICE_FILE_TOO_BIG = "File is too big.";
        public const string FILESERVICE_INVALID_SIGNATURE = "File signature not recognised.";
        public const string FILESERVICE_DIR_NOT_EXIST = "Directory does not exists.";
        public const string FILESERVICE_DIR_EXISTS = "Directory already exists.";
        public const string FILESERVICE_FILE_EXISTS = "File already exists.";
        public const string FILESERVICE_CREATE_UNIQUE_DIR = "Could not create unique wishlist directory name.";
        public const string FILESERVICE_CREATE_UNIQUE_FILENAME = "Could not create unique filename.";
    }
}
