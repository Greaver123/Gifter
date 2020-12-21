using Gifter.DataAccess.Constants;
using System.Collections.Generic;

namespace Gifter.DataAccess.Models
{
    public class GiftType
    {
        public int Id { get; set; }

        public GiftCategory Category { get; set; }

        public ICollection<Gift> Gifts { get; set; }

    }
}