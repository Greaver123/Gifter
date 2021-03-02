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
        private readonly IImageService uploadService;

        public ImageController(IImageService uploadService)
        {
            this.uploadService = uploadService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("upload")]
        public async Task<IActionResult> Post([FromForm] UploadImageDTO uploadImageDTO)
        {
            var operationResult = await uploadService.UploadImageAsync(uploadImageDTO, User.SubjectId());

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
        [HttpGet("{wishId:int}")]
        public async Task<IActionResult> Get(int wishId)
        {
            var operationResult = await uploadService.DownloadImageAsync(wishId, User.SubjectId());

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
        [HttpDelete("{wishId:int}")]
        public async Task<IActionResult> Delete(int wishId)
        {
            return NotFound();
            //var operationResult = await uploadService.Delete(wishId, User.SubjectId());

            //switch (operationResult.Status)
            //{
            //    case OperationStatus.SUCCESS:
            //        return Ok(operationResult);
            //    case OperationStatus.FAIL:
            //        return NotFound(operationResult);
            //    case OperationStatus.ERROR:
            //        return StatusCode(StatusCodes.Status500InternalServerError, operationResult);
            //    default:
            //        return Ok(operationResult);
            //}
        }
    }
}
