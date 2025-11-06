using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQuery : ICommand<ErrorOr<RestaurantResult>>
    {
        public int? RestaurantId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
