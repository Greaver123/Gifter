using Gifter.Extensions;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.DTOS.Wishlist;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly IWishService wishService;

        public WishController(IWishService wishService)
        {
            this.wishService = wishService;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return await wishService.DeleteAsync(id, this.HttpContext.User.SubjectId()) ? Ok() : NotFound();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var result = await wishService.GetAsync(id, this.HttpContext.User.SubjectId());

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AddWishDTO wishDTO)
        {
            var result = await wishService.AddAsync(wishDTO, this.HttpContext.User.SubjectId());

            return result.HasValue ? CreatedAtAction(nameof(Get),
                new { id = result.Value},
                new { id = result.Value }) : BadRequest();
        }
    }
}
