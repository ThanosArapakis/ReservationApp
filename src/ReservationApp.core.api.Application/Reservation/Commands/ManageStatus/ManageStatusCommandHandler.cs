using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Commands.ConfirmAppointment
{
    public class ManageStatusCommandHandler(IReservationRepository _repo) : ICommandHandler<ManageStatusCommand, ErrorOr<PostResponse>>
    {
        public Task<ErrorOr<PostResponse>> HandleAsync(ManageStatusCommand command, CancellationToken cancellationToken = default)
        => _repo.ManageStatus(command.AppointmentId, command.Status, cancellationToken);
    }
}
