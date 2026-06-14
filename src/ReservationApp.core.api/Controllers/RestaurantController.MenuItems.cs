using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;

namespace ReservationApp.core.api.Controllers
{
    /// <summary>
    /// RestaurantController
    /// </summary>
    /// <remarks>Contains Menu Item Endpoints</remarks>
    [Route("restaurant")]
    [ApiController]
    [Tags("MenuItems")]
    public class RestaurantController(IMediatorBus _mediator, ILogger<RestaurantApiController> _logger) : BaseApiController<RestaurantApiController>(_logger)
    {
        [HttpPost("CreateMenuItem")]
        public async Task<ResponseDto> CreateMenuItem([FromBody] CreateMenuItemCommand command)
        {
            return await HandleExceptionAsync<PostResponse>(async () => await _mediator.SendAsync<CreateMenuItemCommand, ErrorOr<PostResponse>>(command));
        }

        [HttpPost("UpdateMenuItem")]
        public async Task<ResponseDto> UpdateMenuItem([FromBody] UpdateMenuItemCommand command)
        {
            return await HandleExceptionAsync<PostResponse>(async () => await _mediator.SendAsync<UpdateMenuItemCommand, ErrorOr<PostResponse>>(command));
        }

        [HttpDelete("DeleteMenuItem/{id}")]
        public async Task<ResponseDto> DeleteMenuItem(int id)
        {
            var command = new ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem.DeleteMenuItemCommand { ItemId = id };
            return await HandleExceptionAsync<DeleteResponse>(async () => await _mediator.SendAsync<DeleteMenuItemCommand, ErrorOr<DeleteResponse>>(command));
        }
    }
}
