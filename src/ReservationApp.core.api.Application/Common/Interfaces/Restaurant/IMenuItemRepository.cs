using ErrorOr;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;

namespace ReservationApp.core.api.Application.Common.Interfaces.Restaurant
{
    public interface IMenuItemRepository
    {
        Task<ErrorOr<PostResponse>> CreateMenuItem(CreateMenuItemCommand command);
        Task<ErrorOr<DeleteResponse>> DeleteMenuItem(DeleteMenuItemCommand command);
        Task<ErrorOr<PostResponse>> UpdateMenuItemAsync(UpdateMenuItemCommand command);
    }
}
