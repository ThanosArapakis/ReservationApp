using ReservationApp.core.api.Application.MenuItem.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Results
{
    public record GetReservationsForUserResult
    (
        int ReservationId,
        int RestaurantId,
        string RestaurantName,
        string RestaurantAddress,
        string UserID,
        string Name,
        string UserEmail,
        int? NumberOfGuests,
        DateTime? ReservationDate,
        string? ReservationStatus,
        List<MenuItemResult> MenuItems
    );
}
