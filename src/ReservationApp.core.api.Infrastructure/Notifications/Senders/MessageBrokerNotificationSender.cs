using ErrorOr;
using Microsoft.Extensions.Logging;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Infrastructure.Notifications.Senders
{
    /// <summary>
    /// Publishes the notification to the message broker (e.g. RabbitMQ). This is the
    /// always-on channel: the factory selects it for every message.
    /// </summary>
    public class MessageBrokerNotificationSender(ILogger<MessageBrokerNotificationSender> _logger) : INotificationSender
    {
        public NotificationChannel Channel => NotificationChannel.MessageBroker;

        public Task<ErrorOr<Success>> SendAsync(NotificationMessage message, CancellationToken cancellationToken)
        {
            // Integration point: publish to RabbitMQ here. Swap this stub for an
            // IConnection/IModel (RabbitMQ.Client) or a MassTransit IPublishEndpoint:
            //
            //     await _publishEndpoint.Publish(message, cancellationToken);
            //
            // Keep the body serialized (JSON) and route via exchange/routing key.
            _logger.LogInformation(
                "Publishing notification to message broker. Recipient={RecipientId}, Subject={Subject}",
                message.RecipientId, message.Subject);

            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }
    }
}
