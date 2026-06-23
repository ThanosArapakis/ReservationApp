namespace ReservationApp.core.api.Application.Common.Notifications
{
    /// <summary>
    /// A channel-agnostic notification. The sender factory inspects this message
    /// to decide which channels it should be delivered through — the broker is
    /// always used, while <see cref="RecipientEmail"/> drives whether an email is sent.
    /// </summary>
    public class NotificationMessage
    {
        /// <summary>Identifier of the recipient (e.g. the reservation's UserId).</summary>
        public string? RecipientId { get; set; }

        /// <summary>Recipient email address. When set, the email channel is added by the factory.</summary>
        public string? RecipientEmail { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        /// <summary>Free-form payload carried to the broker (correlation ids, entity keys, etc.).</summary>
        public IDictionary<string, string>? Metadata { get; set; }
    }
}
