using ReservationApp.core.api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Results
{
    public record RestaurantResult(
      int RestaurantId,
      string Name,
      string Address,
      string PhoneNumber,
      int Capacity,
      List<MenuItemResult> MenuItems
    );
}
