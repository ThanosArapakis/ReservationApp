using ReservationApp.core.api.Application.Common;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Common.Util
{
    internal static class ReservationMapper
    {
        internal static GetReservationsForUserResult ToGetReservationResult (this Reservation reservation) =>
            new(
                reservation.Id,
                reservation.RestaurantId.Value,
                reservation.Restaurant?.Name ?? string.Empty,
                reservation.Restaurant?.Address ?? string.Empty,
                reservation.UserId ?? string.Empty,
                reservation.Name ?? string.Empty,
                reservation.UserEmail ?? string.Empty,
                reservation.NumberOfGuests,
                reservation.ReservationDate,
                ReservationStatus.FromValue(0)!.Description,
                reservation.ReservationMenuItems.Select(rm => rm.MenuItem.ToMenuItemResult()).ToList()
            );
    }
}
