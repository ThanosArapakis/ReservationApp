using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.MenuItem.Results;
using ReservationApp.core.api.Application.Reservation.Commands.CreateReservation;
using ReservationApp.core.api.Application.Reservation.Queries.GetReservationsForUser;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Infrastructure.Common.Util;
using ReservationApp.core.api.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Persistence.Repository
{
    public class ReservationRepository(AppDbContext _db) : IReservationRepository
    {
        public async Task<ErrorOr<CreateReservationResult>> CreateReservation(CreateReservationCommand command, CancellationToken token)
        {
            try
            {
                // Map the command to a Reservation entity
                Reservation reservation = new Reservation
                {
                    RestaurantId = command.RestaurantId,
                    UserId = command.UserId,
                    Name = command.UserName ?? string.Empty,
                    UserEmail = command.UserEmail,
                    UserPhone = command.UserPhone,
                    NumberOfGuests = command.NumberOfGuests,
                    ReservationDate = command.ReservationDate
                };
                _db.Reservations.Add(reservation);

                ReservationMenuItem[] reservationMenuItems = command.MenuItems.Select(mi => new ReservationMenuItem
                {
                    MenuItemId = mi.ItemId,
                    Reservation = reservation
                }).ToArray();
                _db.ReservationMenuItems.AddRange(reservationMenuItems);

                _db.SaveChanges();
                CreateReservationResult result = new CreateReservationResult
                (
                    reservation.RestaurantId.Value,
                    reservation.UserId ?? string.Empty,
                    reservation.NumberOfGuests,
                    reservation.ReservationDate,
                    0
                );
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
