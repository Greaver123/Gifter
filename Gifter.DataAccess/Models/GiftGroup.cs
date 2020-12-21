using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class GiftGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Participant> Participants { get; set; }

        public Event Event { get; set; }

        public ICollection<WishList> WishLists { get; set; }
    }
}
