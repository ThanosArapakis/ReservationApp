using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Application.Restaurant.Queries;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Reservation.Queries.GetReservationsForUser
{
    public record GetReservationQuery(string UserId) : ICommand<ErrorOr<List<GetReservationsForUserResult>>>;

    public class GetReservationsForUserCommandHandler(IReservationRepository _repo) : ICommandHandler<GetReservationQuery, ErrorOr<List<GetReservationsForUserResult>>>
    {
        public async Task<ErrorOr<List<GetReservationsForUserResult>>> HandleAsync(GetReservationQuery command, CancellationToken cancellationToken = default)
        => await _repo.GetReservationsForUserAsync(command);
    }
}
