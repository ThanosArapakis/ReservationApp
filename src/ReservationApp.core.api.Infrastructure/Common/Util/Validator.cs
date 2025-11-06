using ErrorOr;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Domain.Interfaces;
using ReservationApp.core.api.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure.Common.Util
{
    internal static class Validator<T> where T : IBasicObject
    {
        internal static ErrorOr<bool> Validate(T obj, AppDbContext _db)
        {
            if (obj == null) return Error.NotFound(description: "Object is null");
            if (_db.Restaurants.Any(r => r.Name == obj.Name)) return Error.Conflict("Restaurant", "Restaurant already exists.");
            return true;
        }

    }
}
