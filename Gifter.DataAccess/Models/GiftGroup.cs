using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gifter.DataAccess.Models
{
    public class GiftGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column("OwnerId")]
        public int UserId { get; set; }

        public int EventId { get; set; }

        public User User { get; set; }

        public Event Event { get; set; }

        public ICollection<Participant> Participants { get; set; }

        public ICollection<WishList> WishLists { get; set; }
    }
}
