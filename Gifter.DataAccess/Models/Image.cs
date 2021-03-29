using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public int WishId { get; set; }

        public Wish Wish { get; set; }
    }
}
