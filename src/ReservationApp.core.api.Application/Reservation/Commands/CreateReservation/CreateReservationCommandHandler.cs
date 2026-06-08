using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Reservation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Commands.CreateReservation
{
    public class CreateReservationCommandHandler(IReservationRepository _repo) : ICommandHandler<CreateReservationCommand, ErrorOr<CreateReservationResult>>
    {
        public Task<ErrorOr<CreateReservationResult>> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken = default)
        => _repo.CreateReservation(command, cancellationToken);
    }
}
