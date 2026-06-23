using ErrorOr;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Application.Common.Interfaces.Notifications
{
    /// <summary>
    /// Entry point for raising a notification. Callers build a channel-agnostic
    /// <see cref="NotificationMessage"/>; the service uses the
    /// <see cref="INotificationSenderFactory"/> to fan it out across every applicable channel.
    /// </summary>
    public interface INotificationService
    {
        Task<ErrorOr<Success>> NotifyAsync(NotificationMessage message, CancellationToken cancellationToken);
    }
}
