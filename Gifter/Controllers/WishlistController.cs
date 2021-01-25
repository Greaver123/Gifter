using Gifter.Extensions;
using Gifter.Services.DTOS.Wishlist;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        // GET: api/<WishlistController>
        [HttpGet]
        public async Task<IEnumerable<WishlistDTO>> Get()
        {
            var userId = User.SubjectId();
            var wishlists = await wishlistService.GetWishlists(userId);

            return wishlists;
        }

        // GET api/<WishlistController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WishlistController>
        [HttpPost]
        public async Task<ActionResult<WishlistCreateDTO>> Post([FromBody] WishlistCreateDTO wishlistDTO)
        {
            var userId = User.SubjectId();
            var wishlist = await wishlistService.CreateWishlist(wishlistDTO.Title, userId);

            return CreatedAtAction(nameof(Get), new {id = wishlist.Id });
        }

        // PUT api/<WishlistController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WishlistController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.SubjectId();
            await wishlistService.DeleteWishlist(id, userId);
            
            return Ok();
        }
    }
}
