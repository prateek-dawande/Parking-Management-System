using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Models.Domain;

namespace ParkingManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ApplicationDbContext applicatioDbContext;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<AvailabilityDate> AvailabilityDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between User and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Configure one-to-many relationship between ParkingSlot and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ParkingSlot)
                .WithMany(ps => ps.Bookings)
                .HasForeignKey(b => b.SlotId);
        }
    }
}
