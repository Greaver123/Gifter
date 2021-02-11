using Gifter.Common.Exceptions;
using Gifter.Extensions;
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
    public class ImagesController : ControllerBase
    {
        private readonly IUploadService uploadService;

        public ImagesController(IUploadService uploadService)
        {
            this.uploadService = uploadService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDTO uploadImageDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await uploadService.UploadAsync(uploadImageDTO, User.SubjectId());

                    return result ? Ok() : NotFound();

                }

                return BadRequest();
            }
            catch (UploadFileException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{wishId:int}")]
        public async Task<IActionResult> GetImageBase64(int wishId)
        {
            var userid = User.SubjectId();
            var result = await uploadService.DownloadImageAsync(wishId, userid);

            if (result == null) return NotFound();
            var base64 = Convert.ToBase64String(result.Image);

            return Ok("data:image/png;base64," + base64);
        }
    }
}
