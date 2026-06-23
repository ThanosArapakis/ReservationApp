using Azure;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Application.Common;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Notifications;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Results;
using ReservationApp.core.api.Application.Reservation.Commands.CreateReservation;
using ReservationApp.core.api.Application.Reservation.Queries.GetReservationsForUser;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Domain.Request;
using ReservationApp.core.api.Infrastructure.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Persistence.Repository
{
    public class ReservationRepository(AppDbContext _db, IRestaurantRepository _restaurantRepo, INotificationService _notifications) : IReservationRepository
    {

        public async Task<ErrorOr<CreateReservationResult>> CreateReservation(CreateReservationCommand command, CancellationToken token)
        {
            try
            {
                // Map the command to a Reservation entity
                Reservation reservation = command.ToReservation();
                ErrorOr<bool> validation = ReservationValidator.Validate(reservation, _db);
                if (validation.IsError) return validation.Errors;

                //Check capacity for the reservation
                if(!await _restaurantRepo.CheckCapacity(reservation.RestaurantId, reservation.NumberOfGuests.Value, reservation.ReservationDate.Value)) 
                    return CustomErrors.RestaurantCapacityExceeded;

                _db.Reservations.Add(reservation);

                //Add the menu items connected to the Reservation
                ReservationMenuItem[] reservationMenuItems = command.MenuItems.Select(mi => new ReservationMenuItem
                {
                    MenuItemId = mi.ItemId,
                    Reservation = reservation // reference to the newly created reservation
                }).ToArray();
                _db.ReservationMenuItems.AddRange(reservationMenuItems);

                //Adjust the restaurant capacity after the reservation
                await _restaurantRepo.ReduceCapacity(reservation.RestaurantId, reservation.NumberOfGuests.Value, reservation.ReservationDate.Value);

                await _db.SaveChangesAsync();

                // Always publishes to the broker; the factory adds the email channel
                // automatically when the reservation carries a UserEmail.
                await _notifications.NotifyAsync(new NotificationMessage
                {
                    RecipientId = reservation.UserId,
                    RecipientEmail = reservation.UserEmail,
                    Subject = "Reservation received",
                    Body = $"Your reservation for {reservation.NumberOfGuests} guest(s) on {reservation.ReservationDate.Value.ToString("dd/mm/yyyy")} has been received.",
                    Metadata = new Dictionary<string, string>
                    {
                        ["reservationId"] = reservation.Id.ToString(),
                        ["restaurantId"] = reservation.RestaurantId.ToString()
                    }
                }, token);

                CreateReservationResult result = reservation.ToReservationResult();
                return result;
            }
            catch(Exception ex)
            {
                return Error.Failure(description: ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public async Task<ErrorOr<List<GetReservationsForUserResult>>> GetReservationsForUserAsync(GetReservationQuery command)
        {
            try
            {
                List<Reservation> reservations = await _db.Reservations.Where(r => r.UserId == command.UserId)
                    .Include(r => r.Restaurant)
                    .Include(r => r.ReservationMenuItems)
                    .ThenInclude(rm => rm.MenuItem).ToListAsync();

                List<GetReservationsForUserResult> result = reservations.ConvertAll(r => r.ToGetReservationResult()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public async Task<ErrorOr<PostResponse>> ManageStatus(int appointmentId, ReservationStatus newStatus, CancellationToken cancellationToken)
        {
            try
            {
                Reservation? reservation = await GetByIdAsync(appointmentId, cancellationToken);
                if (reservation == null)
                {
                    return CustomErrors.ReservationNotFound;
                }
                reservation.Status = newStatus.Value;
                await _db.SaveChangesAsync(cancellationToken);

                return new PostResponse(reservation.RestaurantId);
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        private async Task<Reservation> GetByIdAsync(int reservationId, CancellationToken cancellationToken)
        {
            Reservation? reservation = await _db.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.ReservationMenuItems)
                .ThenInclude(rm => rm.MenuItem)
                .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {reservationId} not found.");
            }
            return reservation;
        }
    }
}
