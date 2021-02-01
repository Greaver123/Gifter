using Gifter.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Gifter.DataAccess
{
    public class GifterDbContext : DbContext
    {
        public GifterDbContext(DbContextOptions<GifterDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<GiftGroup> GiftGroups { get; set; }

        public DbSet<WishList> Wishlists { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Wish> Wishes { get; set; }

        public DbSet<GiftType> GiftType { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Participant>().HasKey(p => new { p.UserId, p.GriftGroupId });

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithMany(u => u.Participants)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Participant>()
                .HasOne(p => p.GiftGroup)
                .WithMany(u => u.Participants)
                .HasForeignKey(u => u.GriftGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Wish)
            .WithOne(g => g.Reservation)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
