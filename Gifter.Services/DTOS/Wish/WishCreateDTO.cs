using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wish
{
    public class WishCreateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public bool IsNew { get; set; }
    }
}