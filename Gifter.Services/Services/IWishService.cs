﻿using Gifter.Services.Common;
using Gifter.Services.DTOS.Wish;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace Gifter.Services.Services
{
    public interface IWishService
    {
        /// <summary>
        /// Deletes Wish from wishlist
        /// </summary>
        /// <param name="id">Id of wish to be deleted</param>
        /// <param name="userId">Id of user</param>
        /// <returns> True if deleted. False if wish could not be found for given userId.</returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        Task<OperationResult<bool>> DeleteAsync(int id, string userId);

        Task<OperationResult<WishDTO>> AddAsync(AddWishDTO addWishDTO, string userId);

        Task<OperationResult<UpdateWishDTO>> UpdateAsync(UpdateWishDTO wishDTO, string userId);

        Task<OperationResult<WishDTO>> GetAsync(int id, string userId);

        Task<OperationResult<UpdateWishDTO>> PatchAsync(int wishId, JsonPatchDocument<UpdateWishDTO> wishPatch, string userId);
    }
}