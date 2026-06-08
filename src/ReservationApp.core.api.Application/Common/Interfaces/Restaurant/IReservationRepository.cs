using ErrorOr;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.Reservation.Commands.CreateReservation;
using ReservationApp.core.api.Application.Reservation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common.Interfaces.Restaurant
{
    public interface IReservationRepository
    {
        Task<ErrorOr<CreateReservationResult>> CreateReservation(CreateReservationCommand command, CancellationToken token);

    }
}
