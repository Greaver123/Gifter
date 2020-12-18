using Gifter.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Gifter.DataAccess
{
    public class GifterDbContext :DbContext
    {
        public GifterDbContext(DbContextOptions<GifterDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<GiftGroup> GiftGroups { get; set; }

        public DbSet<WishList> Wishlists { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Gift> Gifts { get; set; }

        public DbSet<GiftType> GiftType { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }
    }
}
