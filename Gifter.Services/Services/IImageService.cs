﻿using Gifter.Services.DTOs.Image;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Save Image for given WishId
        /// </summary>
        /// <param name="wishId">Id of wish entity</param>
        /// <param name="filePath">Path for image</param>
        /// <param name="userId">Authenticated userid</param>
        /// <returns>true if operation succeded</returns>
        /// <exception cref="ArgumentException">Thrown when filepath or userId are null/empty or whitespace</exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        Task<bool> AddImageAsync(int wishId, string filePath);
    }
}