namespace Gifter.DataAccess.Models
{
    public class Participant
    {

        public int UserId { get; set; }
        public User User { get; set; }


        public int GriftGroupId { get; set; }
        public GiftGroup GiftGroup { get; set; }
    }
}
