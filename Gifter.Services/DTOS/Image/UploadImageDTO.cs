using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Image
{
    public class UploadImageDTO
    {
        [Required]
        public int WishId { get; set; }

        [Required]
        public IFormFile ImageFile {get;set;}
    }
}
