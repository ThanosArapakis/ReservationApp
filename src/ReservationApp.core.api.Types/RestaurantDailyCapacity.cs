using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Domain
{
    public class RestaurantDailyCapacity
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public DateTime Date { get; set; }
        public int Capacity { get; set; }
    }
}
