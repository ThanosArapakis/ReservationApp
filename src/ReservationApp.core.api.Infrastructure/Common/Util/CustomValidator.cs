//using ErrorOr;
//using ReservationApp.core.api.Domain;
//using ReservationApp.core.api.Infrastructure.Persistence;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace ReservationApp.core.api.Infrastructure.Common.Util
//{
//    internal static class CustomValidator<T> where T : class
//    {
//        internal static ErrorOr<bool> ValidateRestaurant(this T restaurant, AppDbContext _db)
//        {
//            if (restaurant == null) return Error.NotFound(description: "Restaurant object is null");
//            if (_db.Restaurants.Any(r => r.Name == restaurant.Name)) return Error.Conflict("Restaurant", "Restaurant already exists.");
//            return true;
//        }

//        internal static ErrorOr<bool> ValidateMenuItem(this MenuItem menuItem, AppDbContext _db)
//        {
//            if (menuItem == null) return Error.NotFound(description: "MenuItem object is null");
//            if (!_db.Restaurants.Any(r => r.Id == menuItem.RestaurantId)) return Error.NotFound(description: "RestaurantId does not exist.");
//            if (_db.MenuItems.Any(mi => mi.Name == menuItem.Name && mi.RestaurantId == menuItem.RestaurantId)) return Error.Conflict("MenuItem", "MenuItem already exists for this Restaurant.");
//            return true;
//        }

//    }
//}
