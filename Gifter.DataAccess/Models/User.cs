using System.Collections.Generic;

namespace Gifter.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Auth0Id { get; set; }

        public string Auth0Username { get; set; }

        public string Auth0Email { get; set; }


        public ICollection<GiftGroup> GiftGroups { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<WishList> WishLists { get; set; }
    }
}
