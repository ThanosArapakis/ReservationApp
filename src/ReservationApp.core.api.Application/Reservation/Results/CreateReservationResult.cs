using ReservationApp.core.api.Application.MenuItem.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Results
{
    public record CreateReservationResult(
        int RestaurantId,
        string Name,
        int? NumberOfGuests,
        DateTime? ReservationDate,
        string ReservationStatus
    );
}
