using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifter.DataAccess.Models
{
    public class WishList
    {
        public int Id { get; set; }

        public string  Name { get; set; }

        [Column("OwnerId")]
        public int UserId { get; set; }

        public int? GiftGroupId { get; set; }

        public User User { get; set; }

        public GiftGroup GiftGroup { get; set; }
        public ICollection<Gift> Gifts { get; set; }
    }
}