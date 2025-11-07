using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Infrastructure.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Persistence.Repository
{
    public class MenuItemRepository(AppDbContext _db) : IMenuItemRepository
    {

        public async Task<ErrorOr<PostResponse>> CreateMenuItem(CreateMenuItemCommand command)
        {
            MenuItem menuItem = command.ToMenuItem();

            bool exists = _db.Restaurants.Any(r => r.Id == command.RestaurantId);
            if(command.CategoryId > 2) return Error.Validation("Invalid Category", "Category has to be 0 - 2");
            if (exists)
            {
                _db.MenuItems.Add(menuItem);
                await _db.SaveChangesAsync();

                return new PostResponse(menuItem.RestaurantId);
            }
            else return Error.NotFound("NotFound", "Restaurant not found");
        }

        public async Task<ErrorOr<DeleteResponse>> DeleteMenuItem(DeleteMenuItemCommand command)
        {
            MenuItem? menuItem = GetById(command.ItemId);
            if (menuItem == null) return Error.NotFound("NotFound", "MenuItem not found");
            try
            {
                _db.MenuItems.Remove(menuItem);
                await _db.SaveChangesAsync();
                return new DeleteResponse(true);
            }
            catch (Exception ex)
            {
                return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public async Task<ErrorOr<PostResponse>> UpdateMenuItemAsync(UpdateMenuItemCommand command)
        {
            bool connected = _db.Restaurants.Any(r => r.Id == command.RestaurantId);
            bool exists = _db.MenuItems.Any(mi => mi.ItemId == command.ItemId);

            if (command.CategoryId > 2) return Error.Validation("Invalid Category", "Category has to be 0 - 2");
            if (exists && connected)
            {
                try
                {
                    MenuItem? menuItem = GetById(command.ItemId);

                    menuItem!.CategoryId = command.CategoryId;
                    menuItem.Name = command.Name ?? menuItem.Name;
                    menuItem.Description = command.Description ?? menuItem.Description;
                    menuItem.Price = command.Price ?? menuItem.Price;
                    menuItem.Available = command.Available ?? menuItem.Available;
                    menuItem.RestaurantId = command.RestaurantId ?? menuItem.RestaurantId;

                    _db.MenuItems.Update(menuItem);
                    await _db.SaveChangesAsync();

                    return new PostResponse(menuItem.ItemId);
                }
                catch (Exception ex)
                {
                    return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
                }
            }
            return Error.NotFound("NotFound", "MenuItem not found");
        }

        private MenuItem? GetById(int id)
        {
            return _db.MenuItems
                .AsNoTracking()
                .FirstOrDefault(r => r.ItemId == id);
        }
    }
}
