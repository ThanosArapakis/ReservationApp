using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem
{
    public class UpdateMenuItemCommandHandler(IMenuItemRepository _repo) : ICommandHandler<UpdateMenuItemCommand, ErrorOr<PostResponse>>
    {
        public Task<ErrorOr<PostResponse>> HandleAsync(UpdateMenuItemCommand command, CancellationToken cancellationToken = default)
        => _repo.UpdateMenuItemAsync(command);
    }
}
