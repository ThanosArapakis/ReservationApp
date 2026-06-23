using ErrorOr;
using Microsoft.Extensions.Logging;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Notifications;

namespace ReservationApp.core.api.Infrastructure.Notifications.Senders
{
    /// <summary>
    /// Sends the notification as an email. The factory only selects this sender when the
    /// message carries a recipient address, so it never has to short-circuit on a missing one.
    /// </summary>
    public class EmailNotificationSender(ILogger<EmailNotificationSender> _logger) : INotificationSender
    {
        public NotificationChannel Channel => NotificationChannel.Email;

        public Task<ErrorOr<Success>> SendAsync(NotificationMessage message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(message.RecipientEmail))
            {
                // Defensive: the factory shouldn't route here without an address.
                return Task.FromResult<ErrorOr<Success>>(
                    Error.Validation("Notification.Email.MissingRecipient", "No email address on the notification."));
            }

            // Integration point: send the email here. Swap this stub for an SMTP client
            // (System.Net.Mail.SmtpClient) or a provider SDK (SendGrid, Amazon SES):
            //
            //     await _smtpClient.SendMailAsync(BuildMail(message), cancellationToken);
            _logger.LogInformation(
                "Sending email notification to {RecipientEmail}. Subject={Subject}",
                message.RecipientEmail, message.Subject);

            return Task.FromResult<ErrorOr<Success>>(Result.Success);
        }
    }
}
