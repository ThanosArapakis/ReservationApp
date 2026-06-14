using ErrorOr;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationApp.core.api.Application.Common;

namespace ReservationApp.core.api.Infrastructure.Common.Util
{
    internal static class ReservationValidator
    {
        /// <summary>
        /// Validates a reservation object for conflicts and basic requirements
        /// </summary>
        /// <param name="reservation">The reservation to validate</param>
        /// <param name="_db">The database context</param>
        /// <returns>ErrorOr result indicating validation success or failure</returns>
        internal static ErrorOr<bool> Validate(Reservation reservation, AppDbContext _db)
        {
            // Null check
            if (reservation == null)
                return CustomErrors.ReservationIsNull;

            // Validate reservation date
            if (reservation.ReservationDate == null || reservation.ReservationDate < DateTime.Now)
                return CustomErrors.InvalidReservationDate;

            // Validate number of guests
            if (reservation.NumberOfGuests == null || reservation.NumberOfGuests <= 0)
                return CustomErrors.InvalidNumberOfGuests;

            // Validate email
            if (string.IsNullOrWhiteSpace(reservation.UserEmail))
                return CustomErrors.InvalidUserEmail;

            // Validate phone
            if (string.IsNullOrWhiteSpace(reservation.UserPhone))
                return CustomErrors.InvalidUserPhone;

            // Check for duplicate reservations at the same restaurant and date/time
            if (_db.Reservations.Any(r => 
                r.RestaurantId == reservation.RestaurantId && 
                r.ReservationDate == reservation.ReservationDate))
                return CustomErrors.DuplicateReservation;

            // Check if restaurant exists and validate capacity
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == reservation.RestaurantId);
            if (restaurant == null)
                return CustomErrors.RestaurantNotFound;

            if (reservation.NumberOfGuests > restaurant.Capacity)
                return CustomErrors.RestaurantCapacityExceeded;

            // Validate that menu items exist
            //if (reservation.ReservationMenuItems == null || reservation.ReservationMenuItems.Count == 0)
            //    return CustomErrors.NoMenuItemsSelected;

            return true;
        }

        /// <summary>
        /// Validates that a reservation exists
        /// </summary>
        /// <param name="reservationId">The reservation ID to check</param>
        /// <param name="_db">The database context</param>
        /// <returns>ErrorOr result indicating if reservation exists</returns>
        internal static ErrorOr<Reservation> ValidateReservationExists(int reservationId, AppDbContext _db)
        {
            var reservation = _db.Reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null)
                return CustomErrors.ReservationNotFound;

            return reservation;
        }

        /// <summary>
        /// Validates that all menu items in the reservation are available
        /// </summary>
        /// <param name="reservation">The reservation to validate</param>
        /// <param name="_db">The database context</param>
        /// <returns>ErrorOr result indicating menu item availability</returns>
        internal static ErrorOr<bool> ValidateMenuItemsAvailable(Reservation reservation, AppDbContext _db)
        {
            if (reservation.ReservationMenuItems == null || reservation.ReservationMenuItems.Count == 0)
                return CustomErrors.NoMenuItemsSelected;

            var menuItemIds = reservation.ReservationMenuItems.Select(rm => rm.MenuItemId).ToList();
            var availableItems = _db.MenuItems.Where(mi => menuItemIds.Contains(mi.ItemId)).ToList();

            if (availableItems.Count != menuItemIds.Count)
                return CustomErrors.MenuItemNotFound;

            return true;
        }
    }
}
