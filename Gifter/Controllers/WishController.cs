using Gifter.Extensions;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gifter.Controllers
{
    [Authorize]
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
            var operationResult = await wishService.DeleteAsync(id, this.HttpContext.User.SubjectId());

            return operationResult.Data ? Ok(operationResult) : NotFound(operationResult);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var operationResult = await wishService.GetAsync(id, this.HttpContext.User.SubjectId());

            return operationResult.Data != null ? Ok(operationResult) : NotFound(operationResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] AddWishDTO wishDTO)
        {
            var OperationResult = await wishService.AddAsync(wishDTO, this.HttpContext.User.SubjectId());

            return OperationResult.Data != null ? CreatedAtAction(nameof(Get), new { id = OperationResult.Data.Id }, OperationResult) : NotFound(OperationResult);
        }
    }
}
