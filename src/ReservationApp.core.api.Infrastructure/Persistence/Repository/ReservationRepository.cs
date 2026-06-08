using ErrorOr;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.MenuItem.Results;
using ReservationApp.core.api.Application.Reservation.Commands.CreateReservation;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Domain;
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
            // Map the command to a Reservation entity
            Reservation reservation = new Reservation
            {
                RestaurantId = command.RestaurantId,
                UserId = command.UserId,
                Name = command.UserName ?? string.Empty,
                UserEmail = command.UserEmail,
                UserPhone = command.UserPhone,
                NumberOfGuests = command.NumberOfGuests,
                ReservationDate = command.ReservationDate,
                MenuItems = command.MenuItems.Select(mi => new MenuItem
                {
                    ItemId = mi.ItemId,
                    Name = mi.Name,
                    Price = mi.Price
                }).ToList()
            };
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            CreateReservationResult result = new CreateReservationResult
            (
                reservation.RestaurantId.Value,
                reservation.UserId ?? string.Empty,
                reservation.NumberOfGuests,
                reservation.ReservationDate,
                0,
                reservation.MenuItems.Select(mi => new MenuItemResult
                {
                    Name = mi.Name,
                    Price = mi.Price.Value
                }).ToList()
            );
            return result;
        }
    }
}
