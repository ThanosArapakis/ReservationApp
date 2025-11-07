using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Queries;
using ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants;
using ReservationApp.core.api.Application.Restaurant.Results;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReservationApp.core.api.Controllers
{
    [Route("restaurant")]
    [ApiController]
    public class RestaurantApiController(IMediatorBus _mediator, ILogger<RestaurantApiController> _logger) : BaseApiController<RestaurantApiController>(_logger)
    {
        #region Restaurant Endpoints

        [HttpGet("GetRestaurant")]
        public async Task<ResponseDto> GetRestaurant([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? address, [FromQuery] string? phoneNumber)
        {
            GetRestaurantsQuery query = new GetRestaurantsQuery { RestaurantId = id, Name = name, Address = address, PhoneNumber = phoneNumber };

            return await HandleExceptionAsync<RestaurantResult>(async() => await _mediator.SendAsync<GetRestaurantsQuery, ErrorOr<RestaurantResult>>(query));         
        }

        [HttpGet("GetAllRestaurants")]
        public async Task<ResponseDto> GetAllRestaurants()
        {
            return await HandleExceptionAsync<List<RestaurantResult>>(async() => await _mediator.SendAsync<EmptyQuery, ErrorOr<List<RestaurantResult>>>(new EmptyQuery()));           
        }

        [HttpPost("CreateRestaurant")]
        public async Task<ResponseDto> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {
            return await HandleExceptionAsync<PostResponse>(async () => await _mediator.SendAsync<CreateRestaurantCommand, ErrorOr<PostResponse>>(command));
        }

        [HttpPut("UpdateRestaurant")]
        public async Task<ResponseDto> UpdateRestaurant([FromBody] UpdateRestaurantCommand command)
        {
            return await HandleExceptionAsync<PostResponse>(async () => await _mediator.SendAsync<UpdateRestaurantCommand, ErrorOr<PostResponse>>(command));
        }

        [HttpDelete("DeleteRestaurant/{id}")]
        public async Task<ResponseDto> DeleteRestaurant(int id)
        {
            DeleteRestaurantCommand command = new DeleteRestaurantCommand { Id = id };
            return await HandleExceptionAsync<DeleteResponse>(async () => await _mediator.SendAsync<DeleteRestaurantCommand, ErrorOr<DeleteResponse>>(command));
        }

        #endregion

        #region MenuItem Endpoints

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

        #endregion
    }
}
