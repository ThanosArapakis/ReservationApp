using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Infrastructure.Notifications
{
    /// <summary>
    /// Picks the senders for a message at runtime. Every <see cref="INotificationSender"/>
    /// registered in DI is injected here as <c>IEnumerable&lt;INotificationSender&gt;</c> — that
    /// is the "dynamic" part: adding a new channel is just registering another sender, no change
    /// to this factory's callers. The routing rules live in one place below.
    /// </summary>
    public class NotificationSenderFactory(IEnumerable<INotificationSender> _senders) : INotificationSenderFactory
    {
        public IReadOnlyCollection<INotificationSender> GetSenders(NotificationMessage message)
        {
            var selected = new List<INotificationSender>();

            // Rule 1: always publish to the message broker.
            INotificationSender? broker = _senders.FirstOrDefault(s => s.Channel == NotificationChannel.MessageBroker);
            if (broker is not null)
                selected.Add(broker);

            // Rule 2: send an email only when the recipient has an address.
            if (!string.IsNullOrWhiteSpace(message.RecipientEmail))
            {
                INotificationSender? email = _senders.FirstOrDefault(s => s.Channel == NotificationChannel.Email);
                if (email is not null)
                    selected.Add(email);
            }

            return selected;
        }
    }
}
