using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common.Results
{
    public record ResponseDto(object? Result, bool IsSuccess = true, string? Message = null);
}
