using ReservationApp.core.api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Domain
{
    public class Restaurant : IBasicObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Capacity { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
        public List<RestaurantDailyCapacity> DailyCapacities { get; set; } = new();

    }
}
