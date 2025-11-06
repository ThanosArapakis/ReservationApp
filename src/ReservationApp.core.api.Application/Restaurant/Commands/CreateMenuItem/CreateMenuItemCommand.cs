using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Restaurant.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Restaurant.Commands.CreateMenuItem
{
    public class CreateMenuItemCommand : ICommand<ErrorOr<PostResponse>>
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool? Available { get; set; }
        [Required]
        public int RestaurantId { get; set; }
    }
}
