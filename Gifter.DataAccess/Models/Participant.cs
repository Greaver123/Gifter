namespace Gifter.DataAccess.Models
{
    public class Participant
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int GiftGroupId { get; set; }


        public User User { get; set; }

        public GiftGroup GiftGroup { get; set; }
    }
}
