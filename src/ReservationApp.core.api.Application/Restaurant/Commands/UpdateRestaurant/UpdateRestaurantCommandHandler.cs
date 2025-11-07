using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(IRestaurantRepository _restaurantRepository) : ICommandHandler<UpdateRestaurantCommand, ErrorOr<PostResponse>>
    {
        public async Task<ErrorOr<PostResponse>> HandleAsync(UpdateRestaurantCommand command, CancellationToken cancellationToken = default)
        {
            return await _restaurantRepository.UpdateRestaurantAsync(command);
        }
    }
}
