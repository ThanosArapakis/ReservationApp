using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem
{
    public class CreateMenuItemCommandHandler(IMenuItemRepository _repo) : ICommandHandler<CreateMenuItemCommand, ErrorOr<PostResponse>>
    {
        public Task<ErrorOr<PostResponse>> HandleAsync(CreateMenuItemCommand command, CancellationToken cancellationToken = default)
        {
            return _repo.CreateMenuItem(command);
        }
    }
}
