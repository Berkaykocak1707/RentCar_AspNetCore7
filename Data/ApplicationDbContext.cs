using Microsoft.EntityFrameworkCore;
using RentCar_AspNetCore7.Models;

namespace RentCar_AspNetCore7.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins
        {
            get; set;
        }
        public DbSet<Booking> Bookings
        {
            get; set;
        }
        public DbSet<Car> Cars
        {
            get; set;
        }
        public DbSet<City> Cities
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Car>()
                .HasOne(c => c.City)  
                .WithMany()           
                .HasForeignKey(c => c.CityId);  

            modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasColumnType("decimal(18, 2)"); 

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
