using Gifter.Common.Exceptions;
using Gifter.Extensions;
using Gifter.Services.Constants;
using Gifter.Services.DTOS.Image;
using Gifter.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("upload")]
        public async Task<IActionResult> Post([FromForm] UploadImageDTO uploadImageDTO)
        {
            var operationResult = await imageService.UploadImageAsync(uploadImageDTO, User.SubjectId());

            switch (operationResult.Status)
            {
                case OperationStatus.SUCCESS:
                    return Ok(operationResult);
                case OperationStatus.FAIL:
                    return NotFound(operationResult);
                case OperationStatus.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult);
                default:
                    return Ok(operationResult);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{imageId:int}")]
        public async Task<IActionResult> Get(int imageId)
        {
            var operationResult = await imageService.GetImageAsync(imageId, User.SubjectId());

            switch (operationResult.Status)
            {
                case OperationStatus.SUCCESS:
                    return Ok(operationResult);
                case OperationStatus.FAIL:
                    return NotFound(operationResult);
                case OperationStatus.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult);
                default:
                    return Ok(operationResult);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var operationResult = await imageService.DeleteImageAsync(imageId, User.SubjectId());

            switch (operationResult.Status)
            {
                case OperationStatus.SUCCESS:
                    return Ok(operationResult);
                case OperationStatus.FAIL:
                    return NotFound(operationResult);
                case OperationStatus.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult);
                default:
                    return Ok(operationResult);
            }
        }
    }
}
