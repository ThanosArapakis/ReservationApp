using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<MenuItem> MenuItems { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<ReservationMenuItem> ReservationMenuItems { get; set; }

        public virtual DbSet<RestaurantDailyCapacity> RestaurantDailyCapacity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.MenuItems)
            .WithOne(mi => mi.Restaurant)
            .HasForeignKey(mi => mi.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Reservations)
            .WithOne(r => r.Restaurant)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationMenuItem>()
                .HasKey(rm => new { rm.ReservationId, rm.MenuItemId });

            modelBuilder.Entity<ReservationMenuItem>()
                .HasOne(rm => rm.Reservation)
                .WithMany(r => r.ReservationMenuItems)
                .HasForeignKey(rm => rm.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationMenuItem>()
                .HasOne(rm => rm.MenuItem)
                .WithMany(mi => mi.ReservationMenuItems)
                .HasForeignKey(rm => rm.MenuItemId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<RestaurantDailyCapacity>()
                .HasKey(rdc => new { rdc.RestaurantId, rdc.Date });

            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.DailyCapacities)
            .WithOne(rdc => rdc.Restaurant)
            .HasForeignKey(rdc => rdc.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
