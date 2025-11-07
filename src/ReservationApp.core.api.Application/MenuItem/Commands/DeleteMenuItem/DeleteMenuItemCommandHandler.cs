using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem
{
    public class DeleteMenuItemCommandHandler(IMenuItemRepository _repo) : ICommandHandler<DeleteMenuItemCommand, ErrorOr<DeleteResponse>>
    {
        public Task<ErrorOr<DeleteResponse>> HandleAsync(DeleteMenuItemCommand command, CancellationToken cancellationToken = default)
        => _repo.DeleteMenuItem(command);
    }
}
