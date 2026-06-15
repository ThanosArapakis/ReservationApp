using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Reservation.Commands.ConfirmAppointment;

namespace ReservationApp.core.api.Controllers
{
    [Route("restaurant")]
    [ApiController]
    [Tags("Status")]
    public class StatusController(IMediatorBus _mediator, ILogger<RestaurantApiController> _logger) : BaseApiController<RestaurantApiController>(_logger)
    {
        [HttpPost("confirmAppointment/{appointmentId}")]
        public async Task<ResponseDto> ConfirmAppointment(int appointmentId)
        {
            return await HandleExceptionAsync(async () => await _mediator.SendAsync<ConfirmAppointmentCommand, ErrorOr<PostResponse>>(new ConfirmAppointmentCommand(appointmentId)));
        }

    }
}
