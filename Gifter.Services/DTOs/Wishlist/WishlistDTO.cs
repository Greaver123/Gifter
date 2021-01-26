using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gifter.Services.DTOS.Wishlist
{
    public class WishlistDTO
    {
        public string Title { get; set; }

        public List<WishDTO> Wishes { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EventDate { get; set; }

        public string GiftGroupName { get; set; }
    }
}
