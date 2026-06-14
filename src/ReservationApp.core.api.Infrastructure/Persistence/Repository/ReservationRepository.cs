using Azure;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
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
    public class ReservationRepository(AppDbContext _db, IRestaurantRepository _restaurantRepo) : IReservationRepository
    {
        public async Task<ErrorOr<CreateReservationResult>> CreateReservation(CreateReservationCommand command, CancellationToken token)
        {
            try
            {
                // Map the command to a Reservation entity
                Reservation reservation = command.ToReservation();
                ErrorOr<bool> validation = ReservationValidator.Validate(reservation, _db);
                if (validation.IsError) return validation.Errors;
                 
                _db.Reservations.Add(reservation);

                //Add the menu items connected to the Reservation
                ReservationMenuItem[] reservationMenuItems = command.MenuItems.Select(mi => new ReservationMenuItem
                {
                    MenuItemId = mi.ItemId,
                    Reservation = reservation // reference to the newly created reservation
                }).ToArray();

                _db.ReservationMenuItems.AddRange(reservationMenuItems);
                _db.SaveChanges();

                //adjust the capacity of the restaurant
                await _restaurantRepo.ReduceCapacity(reservation.RestaurantId.Value, reservation.NumberOfGuests.Value);

                CreateReservationResult result = reservation.ToReservationResult();
                return result;
            }
            catch(Exception ex)
            {
                return Error.Failure(description: ex.Message + ": " + ex.InnerException.Message);
            }
        }

        public async Task<ErrorOr<List<GetReservationsForUserResult>>> GetReservationsForUserAsync(GetReservationQuery command)
        {
            try
            {
                List<Reservation> reservations = _db.Reservations.Where(r => r.UserId == command.UserId)
                    .Include(r => r.Restaurant)
                    .Include(r => r.ReservationMenuItems)
                    .ThenInclude(rm => rm.MenuItem).ToList();

                List<GetReservationsForUserResult> result = reservations.ConvertAll(r => r.ToGetReservationResult()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message + ": " + ex.InnerException.Message);
            }
        }
    }
}
