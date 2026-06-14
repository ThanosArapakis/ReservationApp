using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Domain
{
    public class ReservationMenuItem
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
