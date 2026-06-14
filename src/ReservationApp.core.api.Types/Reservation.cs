using ReservationApp.core.api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Domain
{
    public class Reservation : IBasicObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public int? NumberOfGuests { get; set; }
        public DateTime? ReservationDate { get; set; }
        public List<ReservationMenuItem> ReservationMenuItems { get; set; } = new List<ReservationMenuItem>();
    }
}
