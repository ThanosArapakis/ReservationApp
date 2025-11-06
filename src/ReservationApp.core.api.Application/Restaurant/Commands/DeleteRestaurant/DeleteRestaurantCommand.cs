using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand : ICommand<ErrorOr<DeleteResponse>>
    {
        public int Id { get; set; }
    }
}
