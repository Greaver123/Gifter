using Gifter.Extensions;
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
        public async Task<IEnumerable<WishlistPreviewDTO>> Get()
        {
            var wishlists = await wishlistService.GetWishlists(User.SubjectId());

            return wishlists;
        }

        // GET api/<WishlistController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistDTO>> Get(int id)
        {
            var wishlist = await wishlistService.GetWishlist(id, User.SubjectId());

            return wishlist != null ? Ok(wishlist): NotFound();
        }

        // POST api/<WishlistController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WishlistCreateDTO>> Post([FromBody] WishlistCreateDTO wishlistDTO)
        {
            if (ModelState.IsValid)
            {
                var operationResult = await wishlistService.CreateWishlist(wishlistDTO.Title, User.SubjectId());

                return CreatedAtAction(nameof(Get), new { id = operationResult.Payload.Id }, operationResult.Payload);
            }

            return BadRequest();
        }

        // PUT api/<WishlistController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WishlistController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var isDeleted = await wishlistService.DeleteWishlist(id, User.SubjectId());

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
