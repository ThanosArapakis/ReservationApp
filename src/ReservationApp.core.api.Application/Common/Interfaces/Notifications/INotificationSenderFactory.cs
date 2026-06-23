using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Application.Common.Interfaces.Notifications
{
    /// <summary>
    /// Resolves, at runtime, which <see cref="INotificationSender"/>s should handle a given
    /// message. This is the factory at the heart of the notification system: it receives every
    /// registered sender from DI and applies the routing rules — the message broker is always
    /// selected, and the email sender is added only when the message carries a recipient address.
    /// </summary>
    public interface INotificationSenderFactory
    {
        IReadOnlyCollection<INotificationSender> GetSenders(NotificationMessage message);
    }
}
