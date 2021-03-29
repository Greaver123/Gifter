namespace Gifter.DataAccess.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int WishId { get; set; }

        public Wish Wish { get; set; }
    }
}
