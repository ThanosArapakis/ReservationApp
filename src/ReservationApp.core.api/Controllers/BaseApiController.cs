using Azure;
using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using ReservationApp.core.api.Application.Common.Util;
using ReservationApp.core.api.Application.Restaurant.Results;

namespace ReservationApp.core.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseApiController<T> : ControllerBase
    {
        private IMediatorBus? _mediator;

        protected IMediatorBus Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediatorBus>() ??
                                                         throw new InvalidOperationException("Mediator not registered");

        private protected readonly ExceptionHandler<T> _exceptionHandler;

        protected BaseApiController(ILogger<T> logger)
        {
            _exceptionHandler = new ExceptionHandler<T>(logger);            
        }

        //TODO: Revisit this method when implementing exception handling strategy
        private protected async Task<ResponseDto> HandleExceptionAsync<TResponse>(Func<Task<ErrorOr<TResponse>>> action)
        where TResponse : class
        {
            return await _exceptionHandler.HandleExceptionAsync(action).ConfigureAwait(false);           
        }
    }
}
