using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wishlist
{
    public class WishDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public bool IsNew { get; set; }
    }
}