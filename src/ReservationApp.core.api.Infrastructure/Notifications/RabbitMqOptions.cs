namespace ReservationApp.core.api.Infrastructure.Notifications
{
    /// <summary>
    /// Strongly-typed view of the "RabbitMq" configuration section. Bound in
    /// <see cref="DependencyInjection"/> and consumed by the broker connection/sender.
    /// </summary>
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";

        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        /// <summary>Exchange the notifications are published to.</summary>
        public string Exchange { get; set; } = "reservation.notifications";

        /// <summary>Queue bound to the exchange (declared so messages aren't dropped before a consumer exists).</summary>
        public string Queue { get; set; } = "ReservationQueue";
    }
}
