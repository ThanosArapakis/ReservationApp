using ErrorOr;
using Microsoft.Extensions.Logging;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Infrastructure.Notifications
{
    /// <summary>
    /// Dispatches a notification across every channel the factory selects. A failure on one
    /// channel does not stop the others; all errors are aggregated and returned.
    /// </summary>
    public class NotificationService(
        INotificationSenderFactory _factory,
        ILogger<NotificationService> _logger) : INotificationService
    {
        public async Task<ErrorOr<Success>> NotifyAsync(NotificationMessage message, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            foreach (INotificationSender sender in _factory.GetSenders(message))
            {
                ErrorOr<Success> result = await sender.SendAsync(message, cancellationToken);
                if (result.IsError)
                {
                    _logger.LogError(
                        "Notification channel {Channel} failed for recipient {RecipientId}: {Errors}",
                        sender.Channel, message.RecipientId, string.Join("; ", result.Errors.Select(e => e.Description)));
                    errors.AddRange(result.Errors);
                }
            }

            return errors.Count > 0 ? errors : Result.Success;
        }
    }
}
