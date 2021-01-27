using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [Required]
        public string  Name { get; set; }

        //[Column("OwnerId")]
        public int UserId { get; set; }
       
        public User User { get; set; }
        
        public int? GiftGroupId { get; set; }

        public GiftGroup GiftGroup { get; set; }

        public ICollection<Wish> Wishes { get; set; }
    }
}