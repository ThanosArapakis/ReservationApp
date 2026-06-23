using ErrorOr;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Application.Common.Interfaces.Notifications
{
    /// <summary>
    /// Delivers a notification over a single channel. One implementation exists per
    /// <see cref="NotificationChannel"/> (broker, email, ...). All implementations are
    /// registered in DI so the <see cref="INotificationSenderFactory"/> can resolve the
    /// whole set and pick the applicable ones at runtime.
    /// </summary>
    public interface INotificationSender
    {
        /// <summary>The channel this sender is responsible for.</summary>
        NotificationChannel Channel { get; }

        Task<ErrorOr<Success>> SendAsync(NotificationMessage message, CancellationToken cancellationToken);
    }
}
