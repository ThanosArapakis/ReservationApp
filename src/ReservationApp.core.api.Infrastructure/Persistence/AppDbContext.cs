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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.MenuItems)
            .WithOne(mi => mi.Restaurant)
            .HasForeignKey(mi => mi.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationMenuItem>()
                .HasKey(rm => new { rm.ReservationId, rm.MenuItemId });

            modelBuilder.Entity<ReservationMenuItem>()
                .HasOne(rm => rm.Reservation)
                .WithMany(r => r.ReservationMenuItems)
                .HasForeignKey(rm => rm.ReservationId);

            modelBuilder.Entity<ReservationMenuItem>()
                .HasOne(rm => rm.MenuItem)
                .WithMany(mi => mi.ReservationMenuItems)
                .HasForeignKey(rm => rm.MenuItemId);
        }
    }
}
