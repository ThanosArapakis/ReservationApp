using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : ICommand<ErrorOr<PostResponse>>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        public int Capacity { get; set; }
    }
}
