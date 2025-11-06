using AutoMapper;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants;
using ReservationApp.core.api.Application.Restaurant.Results;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Infrastructure.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReservationApp.core.api.Infrastructure.Persistence.Repository
{
    public class RestaurantRepository(AppDbContext _db) : IRestaurantRepository
    {

        public async Task<ErrorOr<RestaurantResult>> GetRestaurant(GetRestaurantsQuery query, List<Expression<Func<Domain.Restaurant, bool>>> filters)
        {
            try
            {
                Restaurant? result = new Restaurant();
                if (filters.Count > 0) result = GetByFilters(filters);
                else result = GetById(query.RestaurantId.Value);

                if (result == null) return Error.NotFound("NotFound", "Restaurant not found");
                else
                {
                    return result.ToRestaurantResult();
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
            }

        }

        public async Task<ErrorOr<List<RestaurantResult>>> GetAllRestaurants()
        {
            try
            {
                List<Restaurant> result = _db.Restaurants.Include(r => r.MenuItems).ToList();
                return result.ConvertAll(RepoMapper.ToRestaurantResult);
            }
            catch (Exception ex)
            {
                return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public async Task<ErrorOr<PostResponse>> CreateRestaurant(CreateRestaurantCommand command)
        {
            try
            {
                Restaurant restaurant = command.ToRestaurant();

                ErrorOr<bool> validate = Validator<Restaurant>.Validate(restaurant, _db);

                if (!validate.IsError)
                {
                    _db.Restaurants.Add(restaurant);
                    await _db.SaveChangesAsync();

                    return new PostResponse(restaurant.Id);
                }
                else return validate.Errors;        

            }
            catch (Exception ex)
            {
                return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public async Task<ErrorOr<PostResponse>> CreateMenuItem(CreateMenuItemCommand command)
        {
            MenuItem menuItem = command.ToMenuItem();

            bool exists = _db.Restaurants.Any(r => r.Id == command.RestaurantId);               
            if (exists)
            {
                _db.MenuItems.Add(menuItem);
                await _db.SaveChangesAsync();

                return new PostResponse(menuItem.RestaurantId);
            }
            else return Error.NotFound("NotFound", "Restaurant not found");
        }


        public async Task<ErrorOr<DeleteResponse>> DeleteRestaurant(DeleteRestaurantCommand command)
        {
            Restaurant? restaurant = GetById(command.Id);
            if (restaurant == null) return Error.NotFound("NotFound", "Restaurant not found");
            try
            {
                var result = _db.Remove(restaurant);
                await _db.SaveChangesAsync();
                return new DeleteResponse(true);
            }
            catch (Exception ex)
            {
                return Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message);
            }
        }

        public Task<ErrorOr<PostResponse>> UpdateRestaurantAsync(UpdateRestaurantCommand command)
        {
            Restaurant? restaurant = GetById(command.Id);
            if (restaurant == null) return Task.FromResult<ErrorOr<PostResponse>>(Error.NotFound("NotFound", "Restaurant not found"));
            try
            {
                restaurant.Name = command.Name;
                restaurant.Address = command.Address;
                restaurant.PhoneNumber = command.PhoneNumber;
                restaurant.Capacity = command.Capacity;
                restaurant.MenuItems = command.MenuItems != null ? command.MenuItems.ConvertAll(RepoMapper.ToMenuItem) : restaurant.MenuItems;


                _db.Restaurants.Update(restaurant);
                _db.SaveChanges();
                return Task.FromResult<ErrorOr<PostResponse>>(new PostResponse(restaurant.Id));
            }
            catch (Exception ex)
            {
                return Task.FromResult<ErrorOr<PostResponse>>(Error.Failure("Database Exception", ex.Message + ": " + ex.InnerException?.Message));
            }

        }

        private Restaurant? GetById(int id)
        {
            return _db.Restaurants
                .AsNoTracking()
                .Include(r => r.MenuItems)
                .FirstOrDefault(r => r.Id == id);
        }

        private Restaurant? GetByFilters(List<Expression<Func<Domain.Restaurant, bool>>> filters)
        {
            Restaurant? result = new Restaurant();
            IQueryable<Restaurant> queryable = _db.Restaurants
                        .AsNoTracking()
                        .Include(r => r.MenuItems);
            foreach (var filter in filters)
            {
                queryable = queryable.Where(filter);
            }
            result = queryable.FirstOrDefault();
            return result;
        }
    }
}
