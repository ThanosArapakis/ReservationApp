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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.MenuItems)      // κάθε MenuItem έχει ένα Restaurant
            .WithOne(mi => mi.Restaurant)       // κάθε Restaurant έχει πολλά MenuItems
            .HasForeignKey(mi => mi.RestaurantId)  // FK property
            .OnDelete(DeleteBehavior.Cascade);     // αν σβήσεις Restaurant, σβήνονται και τα MenuItems

            //modelBuilder.Entity<Reservation>()
            //.HasOne(res => res.Restaurant)
            //.WithMany()
            //.HasForeignKey(res => res.RestaurantId)
            //.OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Reservation>()
            .HasMany(r => r.MenuItems)
            .WithOne(mi => mi.Reservation)
            .HasForeignKey(res => res.ReservationId)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
