using ErrorOr;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common.Interfaces.Restaurant
{
    public interface IRestaurantRepository
    {
        Task<ErrorOr<PostResponse>> CreateRestaurant(CreateRestaurantCommand command);

        Task<ErrorOr<RestaurantResult>> GetRestaurant(GetRestaurantsQuery query, List<Expression<Func<Domain.Restaurant, bool>>> filters);

        Task<ErrorOr<List<RestaurantResult>>> GetAllRestaurants();
        Task<ErrorOr<DeleteResponse>> DeleteRestaurant(DeleteRestaurantCommand command);
        Task<ErrorOr<PostResponse>> UpdateRestaurantAsync(UpdateRestaurantCommand command);
        Task<bool> CheckCapacity(int restaurantId, int numberOfGuests, DateTime reservationDate);
        Task ReduceCapacity(int restaurantId, int numberOfGuests, DateTime reservationDate);

    }
}
