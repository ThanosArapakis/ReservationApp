using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Common.Results;

namespace ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(IRestaurantRepository _repo) : ICommandHandler<CreateRestaurantCommand, ErrorOr<PostResponse>>
    {
        public async Task<ErrorOr<PostResponse>> HandleAsync(CreateRestaurantCommand command, CancellationToken cancellationToken = default)
        {
           
            return await _repo.CreateRestaurant(command);
        }
    }
}
