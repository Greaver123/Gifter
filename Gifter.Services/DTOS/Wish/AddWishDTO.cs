using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wish
{
    public class AddWishDTO
    {
        [Required]
        public int WishlistId { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public string URL { get; set; }
    }
}
