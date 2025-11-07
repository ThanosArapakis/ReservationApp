using ReservationApp.core.api.Application.Common;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Results;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Results;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Common.Util
{
    internal static class RepoMapper
    {
        internal static Restaurant ToRestaurant(this CreateRestaurantCommand input) =>
            new()
            {
                Name = input.Name,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                Capacity = input.Capacity
            };

        internal static MenuItem ToMenuItem(this MenuItemDTO input) =>
            new()
            {
                ItemId = input.ItemId,
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                CategoryId = input.CategoryId,
                Available = input.Available ?? true
            };

        internal static MenuItem ToMenuItem(this CreateMenuItemCommand input) =>
            new()
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                RestaurantId = input.RestaurantId,
                CategoryId = input.CategoryId,
                Available = input.Available ?? true
            };

        internal static RestaurantResult ToRestaurantResult(this Restaurant input) =>
            new(
                input.Id,
                input.Name,
                input.Address,
                input.PhoneNumber,
                input.Capacity,
                input.MenuItems.ConvertAll(x => x.ToMenuItemResult())
            );

        internal static MenuItemResult ToMenuItemResult(this MenuItem input) =>
            new()
            {
                CategoryId = input.CategoryId,
                CategoryName = Category.FromValue(input.CategoryId)!.Description,
                Name = input.Name,
                Description = input.Description,
                Price = input.Price.Value,
                Available = input.Available
            };
    }
}
