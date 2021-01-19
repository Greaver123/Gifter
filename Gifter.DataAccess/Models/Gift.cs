﻿using System.ComponentModel.DataAnnotations;

namespace Gifter.DataAccess.Models
{
    public class Gift
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public string ImageUrl { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public int WishListId { get; set; }

        public WishList WishList { get; set; }

        public int GiftTypeId { get; set; }

        public GiftType GiftType { get; set; }

        public Reservation Reservation { get; set; }
    }
}