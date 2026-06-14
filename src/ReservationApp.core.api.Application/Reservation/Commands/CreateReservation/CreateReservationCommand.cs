using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Commands.CreateReservation
{
    public class CreateReservationCommand : ICommand<ErrorOr<CreateReservationResult>> 
    {
        //Restaurant
        public int RestaurantId { get; set; }

        //User Info
        public string? UserId { get; set; }
        public string? UserName { get; set; } = null;
        public string? UserEmail { get; set; } = null;
        public string? UserPhone { get; set; } = null;

        public int? NumberOfGuests { get; set; }
        public DateTime ReservationDate { get; set; }
        public List<MenuItemDTO> MenuItems { get; set; } = new List<MenuItemDTO>();

    }
}
