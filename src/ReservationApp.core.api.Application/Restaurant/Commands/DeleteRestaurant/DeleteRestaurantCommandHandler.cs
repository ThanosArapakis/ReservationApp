using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(IRestaurantRepository _repo) : ICommandHandler<DeleteRestaurantCommand, ErrorOr<DeleteResponse>>
    {
        public Task<ErrorOr<DeleteResponse>> HandleAsync(DeleteRestaurantCommand command, CancellationToken cancellationToken = default)
        {
            return _repo.DeleteRestaurant(command);
        }
    }
}
