using Gifter.Extensions;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wishlist;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        // GET: api/<WishlistController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var operationResult = await wishlistService.GetWishlists(User.SubjectId());

            return Ok(operationResult);
        }

        // GET api/<WishlistController>/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var operationResult = await wishlistService.GetWishlist(id, User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS? Ok(operationResult) : NotFound(operationResult);
        }

        // POST api/<WishlistController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] WishlistCreateDTO wishlistDTO)
        {
            var operationResult = await wishlistService.CreateWishlist(wishlistDTO.Title, User.SubjectId());

            return CreatedAtAction(nameof(Get), new { id = operationResult.Data.Id }, operationResult);
        }

        // PUT api/<WishlistController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] WishlistEditDTO wishlistEditDTO)
        {
            var operationResult = await wishlistService.BulkEditWishlist(wishlistEditDTO, User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS ? Ok(operationResult) : NotFound(operationResult);
        }

        // DELETE api/<WishlistController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var operationResult = await wishlistService.DeleteWishlist(id, User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS ? Ok(operationResult) : NotFound(operationResult);
        }
    }
}
