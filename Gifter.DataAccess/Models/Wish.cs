using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class Wish
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public int WishListId { get; set; }

        public WishList WishList { get; set; }

        public Reservation Reservation { get; set; }

        public Image Image { get; set; }   
    }
}