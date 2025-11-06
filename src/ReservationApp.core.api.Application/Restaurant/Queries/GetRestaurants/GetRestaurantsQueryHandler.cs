using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQueryHandler(IRestaurantRepository _repo) : ICommandHandler<GetRestaurantsQuery, ErrorOr<RestaurantResult>>
    {
        public Task<ErrorOr<RestaurantResult>> HandleAsync(
            GetRestaurantsQuery query,
            CancellationToken cancellationToken = default)
        {
            var filters = new List<Expression<Func<Domain.Restaurant, bool>>>();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                filters.Add(r => r.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                filters.Add(r => r.Address.Contains(query.Address));
            }

            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
            {
                filters.Add(r => r.PhoneNumber.Contains(query.PhoneNumber));
            }

            if (!query.RestaurantId.HasValue && filters.Count == 0)
                return Task.FromResult<ErrorOr<RestaurantResult>>(
                    Error.Validation("InvalidQuery", "At least one filter or RestaurantId must be provided.")
                );

            return _repo.GetRestaurant(query, filters);
        }
    }
}
