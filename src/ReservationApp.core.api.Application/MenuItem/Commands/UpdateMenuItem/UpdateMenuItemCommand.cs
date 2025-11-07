using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem
{
    public class UpdateMenuItemCommand : ICommand<ErrorOr<PostResponse>>
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Available { get; set; }
        public int? RestaurantId { get; set; }
    }
}
