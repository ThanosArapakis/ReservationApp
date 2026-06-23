using System.Text.Json;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Infrastructure.Notifications.Senders
{
    /// <summary>
    /// Publishes the notification to RabbitMQ. This is the always-on channel: the factory
    /// selects it for every message. The body is JSON, routed via the configured exchange.
    /// </summary>
    public class MessageBrokerNotificationSender(
        IRabbitMqConnection _connection,
        IOptions<RabbitMqOptions> _options,
        ILogger<MessageBrokerNotificationSender> _logger) : INotificationSender
    {
        private readonly RabbitMqOptions _cfg = _options.Value;

        public NotificationChannel Channel => NotificationChannel.MessageBroker;

        public async Task<ErrorOr<Success>> SendAsync(NotificationMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await using var channel = await _connection.CreateChannelAsync(cancellationToken);

                // Declare topology so publishing works even before a consumer has run.
                // Fanout: every bound queue gets a copy; routing key is ignored.
                await channel.ExchangeDeclareAsync(
                    _cfg.Exchange, ExchangeType.Fanout, durable: true, autoDelete: false,
                    cancellationToken: cancellationToken);
                await channel.QueueDeclareAsync(
                    _cfg.Queue, durable: true, exclusive: false, autoDelete: false,
                    cancellationToken: cancellationToken);
                await channel.QueueBindAsync(
                    _cfg.Queue, _cfg.Exchange, routingKey: string.Empty,
                    cancellationToken: cancellationToken);

                var body = JsonSerializer.SerializeToUtf8Bytes(message);
                var properties = new BasicProperties
                {
                    ContentType = "application/json",
                    DeliveryMode = DeliveryModes.Persistent,
                };

                await channel.BasicPublishAsync(
                    exchange: _cfg.Exchange,
                    routingKey: string.Empty,
                    mandatory: false,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellationToken);

                _logger.LogInformation(
                    "Published notification to broker. Recipient={RecipientId}, Subject={Subject}",
                    message.RecipientId, message.Subject);

                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish notification to RabbitMQ.");
                return Error.Failure("Notification.Broker.PublishFailed", ex.Message);
            }
        }
    }
}
