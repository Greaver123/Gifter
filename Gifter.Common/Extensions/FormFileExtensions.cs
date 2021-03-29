using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gifter.Common.Extensions
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        private static readonly int[] PngSignature = new int[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        private static readonly int[] JpgSignature = new int[] { 255, 216, 255, 224 };

        private static readonly int[] GifSignature = new int[] { 71, 73, 70, 56 };
        
        private static Dictionary<string, int[]> Signatures = new Dictionary<string, int[]> {
            {"png", PngSignature },
            {"jpg", JpgSignature },
            {"gif", GifSignature },
        };

        public static bool IsImage(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        }

        public static bool IsPNG(this IFormFile postedFile)
        {
            using (var stream = postedFile.OpenReadStream())
            {
                var fileMagicNumber = new List<int>();

                foreach (var byteNum in PngSignature)
                {
                    fileMagicNumber.Add(stream.ReadByte());
                }

                return Enumerable.SequenceEqual(PngSignature, fileMagicNumber);
            }
        }

        public static bool IsGIF(this IFormFile postedFile)
        {
            using (var stream = postedFile.OpenReadStream())
            {
                var fileMagicNumber = new List<int>();

                foreach (var byteNum in PngSignature)
                {
                    fileMagicNumber.Add(stream.ReadByte());
                }

                return Enumerable.SequenceEqual(GifSignature, fileMagicNumber);
            }
        }

        public static bool IsJPG(this IFormFile postedFile)
        {
            using (var stream = postedFile.OpenReadStream())
            {
                var fileMagicNumber = new List<int>();

                foreach (var byteNum in JpgSignature)
                {
                    fileMagicNumber.Add(stream.ReadByte());
                }

                return Enumerable.SequenceEqual(JpgSignature, fileMagicNumber);
            }
        }

        /// <summary>
        /// Gets extension string if match one of supported extensions: PNG, JPG, GIF.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>A file extension string, or null when could not determine extension (unknown or file size less than 8 bytes).</returns>
        public static string TryGetImageExtension(this IFormFile postedFile)
        {
            var fileMagicNumber = new List<int>();

            using (var stream = postedFile.OpenReadStream())
            {
                //Read first 8 bytes of file
                for (int i = 0; i < 8; i++)
                {
                    var value = stream.ReadByte();
                    if (value == -1) return null;
                    fileMagicNumber.Add(value);
                }
            }

            foreach (var signature in Signatures)
            {
                switch (signature.Key)
                {
                    case "png":
                        if (Enumerable.SequenceEqual(fileMagicNumber.Take(8), signature.Value))
                        {
                            return ".png";
                        }
                        break;
                    case "jpg":
                        if (Enumerable.SequenceEqual(fileMagicNumber.Take(4), signature.Value))
                        {
                            return ".jpg";
                        }
                        break;
                    case "gif":
                        if (Enumerable.SequenceEqual(fileMagicNumber.Take(4), signature.Value))
                        {
                            return ".gif";
                        }
                        break;
                }
            }

            return null;
        }
    }
}
