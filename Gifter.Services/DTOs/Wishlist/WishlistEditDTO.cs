using Gifter.Services.DTOS.Wish;
using System.Collections.Generic;

namespace Gifter.Services.DTOS.Wishlist
{
    public class WishlistEditDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<WishCreateDTO> Wishes { get; set; }

        public int GiftGroupId { get; set; }
    }
}