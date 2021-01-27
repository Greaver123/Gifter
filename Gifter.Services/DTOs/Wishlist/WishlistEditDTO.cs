using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wishlist
{
    public class WishlistEditDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<WishDTO> Wishes { get; set; }

        public int GiftGroupId { get; set; }
    }
}