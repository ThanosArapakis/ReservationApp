using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Reservation.Commands;
using ReservationApp.core.api.Application.Reservation.Commands.CreateReservation;
using ReservationApp.core.api.Application.Reservation.Queries.GetReservationsForUser;
using ReservationApp.core.api.Application.Reservation.Results;
using ReservationApp.core.api.Application.Restaurant.Queries;
using ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants;
using ReservationApp.core.api.Application.Restaurant.Results;

namespace ReservationApp.core.api.Controllers
{
    [Route("reservation")]
    [ApiController]
    public class ReservationController(IMediatorBus _mediator, ILogger<ReservationController> _logger) : BaseApiController<ReservationController>(_logger)
    {
        protected EmptyQuery emptyQuery = new EmptyQuery();

        [HttpPost("createReservation")]
        public async Task<ResponseDto> CreateReservation(CreateReservationCommand command)
        {
            return await HandleExceptionAsync(async () => await _mediator.SendAsync<CreateReservationCommand, ErrorOr<CreateReservationResult>>(command));
        }

        [HttpGet("getReservationsForUser")]
        public async Task<ResponseDto> GetReservationsForUser([FromQuery] string userId)
        {
            return await HandleExceptionAsync(async () => await _mediator.SendAsync<GetReservationQuery, ErrorOr<List<GetReservationsForUserResult>>>(new GetReservationQuery(userId)));
        }

    }
}
