namespace ReservationApp.core.api.Application.Common.Notifications
{
    /// <summary>
    /// Channel through which a notification can be delivered.
    /// Used by the sender factory as the routing key when selecting which
    /// <see cref="Interfaces.Notifications.INotificationSender"/> to use.
    /// </summary>
    public enum NotificationChannel
    {
        /// <summary>Always-on transport: publishes the message to the broker (e.g. RabbitMQ).</summary>
        MessageBroker,
        Email,
        Sms,
        Push,
        InApp
    }
}
