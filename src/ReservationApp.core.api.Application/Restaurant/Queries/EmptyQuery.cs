using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReservationApp.core.api.Application.Restaurant.Queries
{
    public class EmptyQuery : ICommand<ErrorOr<List<RestaurantResult>>>
    {
    }
}
