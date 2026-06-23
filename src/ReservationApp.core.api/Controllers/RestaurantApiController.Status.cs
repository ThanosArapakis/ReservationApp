using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Reservation.Commands.ConfirmAppointment;

namespace ReservationApp.core.api.Controllers;

[Route("restaurant")]
[ApiController]
[Tags("Status")]
public class StatusController(IMediatorBus _mediator, ILogger<RestaurantApiController> _logger) : BaseApiController<RestaurantApiController>(_logger)
{
    [HttpPost("confirmAppointment/{appointmentId}")]
    public async Task<ResponseDto> ConfirmAppointment(int appointmentId)
    {
        return await HandleExceptionAsync(async () => await _mediator.SendAsync<ManageStatusCommand, ErrorOr<PostResponse>>(new ManageStatusCommand(appointmentId, ReservationStatus.Confirmed)));
    }

    [HttpPost("cancelAppointment/{appointmentId}")]
    public async Task<ResponseDto> CancelAppointment(int appointmentId)
    {
        return await HandleExceptionAsync(async () => await _mediator.SendAsync<ManageStatusCommand, ErrorOr<PostResponse>>(new ManageStatusCommand(appointmentId, ReservationStatus.Cancelled)));
    }

    [HttpPost("completeAppointment/{appointmentId}")]
    public async Task<ResponseDto> CompleteAppointment(int appointmentId)
    {
        return await HandleExceptionAsync(async () => await _mediator.SendAsync<ManageStatusCommand, ErrorOr<PostResponse>>(new ManageStatusCommand(appointmentId, ReservationStatus.Completed)));
    }

    [HttpPost("noShowAppointment/{appointmentId}")]
    public async Task<ResponseDto> NoShowAppointment(int appointmentId)
    {
        return await HandleExceptionAsync(async () => await _mediator.SendAsync<ManageStatusCommand, ErrorOr<PostResponse>>(new ManageStatusCommand(appointmentId, ReservationStatus.NoShow)));
    }

    //[HttpPost("rescheduleAppointment/{appointmentId}")]
    //public async Task<ResponseDto> RescheduleAppointment(int appointmentId)
    //{
    //    return await HandleExceptionAsync(async () => await _mediator.SendAsync<ManageStatusCommand, ErrorOr<PostResponse>>(new ManageStatusCommand(appointmentId, ReservationStatus.Rescheduled)));
    //}
}
