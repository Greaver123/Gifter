using Gifter.DataAccess.Constants;

namespace Gifter.DataAccess.Models
{
    public class GiftType
    {
        public int Id { get; set; }

        public GiftCategory Category { get; set; }

    }
}