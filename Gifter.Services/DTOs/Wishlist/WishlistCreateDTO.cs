using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wishlist
{
    public class WishlistCreateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 1)]
        public string Title { get; set; }
    }
}