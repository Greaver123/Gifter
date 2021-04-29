using Gifter.Extensions;
using Gifter.Filters;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Wish;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

            return operationResult.Status == OperationStatus.SUCCESS ? Ok(operationResult) : NotFound(operationResult);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var operationResult = await wishService.GetAsync(id, this.HttpContext.User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS? Ok(operationResult) : NotFound(operationResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] AddWishDTO wishDTO)
        {
            var operationResult = await wishService.AddAsync(wishDTO, this.HttpContext.User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS? CreatedAtAction(nameof(Get), new { id = operationResult.Data.Id }, operationResult) : NotFound(operationResult);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] UpdateWishDTO wishDTO)
        {
            var operationResult = await wishService.UpdateAsync(wishDTO, this.HttpContext.User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS ? Ok(operationResult): NotFound(operationResult);
        }

        [ServiceFilter(typeof(ValidatePatchFilter<UpdateWishDTO>))]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]int id, [FromBody] JsonPatchDocument<UpdateWishDTO> patchWishDocument)
        {
            var operationResult = await wishService.PatchAsync(id, patchWishDocument, this.HttpContext.User.SubjectId());

            return operationResult.Status == OperationStatus.SUCCESS ? Ok(operationResult) : NotFound(operationResult);
        }
    }
}
