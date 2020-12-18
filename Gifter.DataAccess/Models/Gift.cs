namespace Gifter.DataAccess.Models
{
    public class Gift
    {
        public int Id { get; set; }

        public int WishListId { get; set; }

        public int GiftTypeId { get; set; }

        public GiftType GiftType { get; set; }

        public WishList WishList { get; set; }
    }
}