using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common
{
    public class CustomErrors
    {
        // Validation Errors
        public static Error ReservationIsNull => Error.Validation(
            code: "Reservation.IsNull",
            description: "Reservation object cannot be null.");

        public static Error InvalidReservationDate => Error.Validation(
            code: "Reservation.InvalidDate",
            description: "Reservation date must not be null and has to be in the future.");

        public static Error InvalidNumberOfGuests => Error.Validation(
            code: "Reservation.InvalidGuests",
            description: "Number of guests must be greater than zero.");

        public static Error InvalidUserEmail => Error.Validation(
            code: "Reservation.InvalidEmail",
            description: "User email is invalid or missing.");

        public static Error InvalidUserPhone => Error.Validation(
            code: "Reservation.InvalidPhone",
            description: "User phone number is invalid or missing.");

        public static Error NoMenuItemsSelected => Error.Validation(
            code: "Reservation.NoMenuItems",
            description: "At least one menu item must be selected for the reservation.");

        // Conflict Errors
        public static Error DuplicateReservation => Error.Conflict(
            code: "Reservation.Duplicate",
            description: "A reservation for this restaurant at the same date and time already exists.");

        public static Error RestaurantCapacityExceeded => Error.Conflict(
            code: "Reservation.CapacityExceeded",
            description: "The number of guests exceeds the restaurant's capacity.");

        public static Error ReservationTimeSlotUnavailable => Error.Conflict(
            code: "Reservation.SlotUnavailable",
            description: "The requested time slot is not available for this restaurant.");

        // Not Found Errors
        public static Error ReservationNotFound => Error.NotFound(
            code: "Reservation.NotFound",
            description: "The requested reservation was not found.");

        public static Error RestaurantNotFound => Error.NotFound(
            code: "Reservation.RestaurantNotFound",
            description: "The specified restaurant was not found.");

        public static Error MenuItemNotFound => Error.NotFound(
            code: "Reservation.MenuItemNotFound",
            description: "One or more selected menu items were not found.");

        public static Error UserNotFound => Error.NotFound(
            code: "Reservation.UserNotFound",
            description: "The specified user was not found.");

        // Failure Errors
        public static Error FailedToCreateReservation => Error.Failure(
            code: "Reservation.CreationFailed",
            description: "Failed to create the reservation. Please try again.");

        public static Error FailedToUpdateReservation => Error.Failure(
            code: "Reservation.UpdateFailed",
            description: "Failed to update the reservation. Please try again.");

        public static Error FailedToDeleteReservation => Error.Failure(
            code: "Reservation.DeleteFailed",
            description: "Failed to delete the reservation. Please try again.");

        public static Error FailedToRetrieveReservations => Error.Failure(
            code: "Reservation.RetrievalFailed",
            description: "Failed to retrieve reservations. Please try again.");

        // Unauthorized Errors
        public static Error UnauthorizedReservationAccess => Error.Unauthorized(
            code: "Reservation.Unauthorized",
            description: "You are not authorized to access this reservation.");

        public static Error UnauthorizedReservationModification => Error.Unauthorized(
            code: "Reservation.UnauthorizedModification",
            description: "You are not authorized to modify this reservation.");
    }
}
