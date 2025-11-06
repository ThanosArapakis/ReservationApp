using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants
{
    public class GetAllRestaurantsQueryHandler(IRestaurantRepository _repo) : ICommandHandler<EmptyQuery, ErrorOr<List<RestaurantResult>>>
    {
        public Task<ErrorOr<List<RestaurantResult>>> HandleAsync(EmptyQuery command, CancellationToken cancellationToken = default)
        {
            return _repo.GetAllRestaurants();
        }
    }
}
