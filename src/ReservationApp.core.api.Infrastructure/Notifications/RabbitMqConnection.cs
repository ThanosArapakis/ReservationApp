using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ReservationApp.core.api.Infrastructure.Notifications
{
    /// <summary>
    /// Owns a single long-lived RabbitMQ <see cref="IConnection"/> for the app and hands
    /// out short-lived channels. Registered as a singleton; channels are cheap and not
    /// thread-safe, so callers create one per publish and dispose it.
    /// </summary>
    public interface IRabbitMqConnection : IAsyncDisposable
    {
        Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);
    }

    public sealed class RabbitMqConnection(IOptions<RabbitMqOptions> options) : IRabbitMqConnection
    {
        private readonly RabbitMqOptions _options = options.Value;
        private readonly SemaphoreSlim _gate = new(1, 1);
        private IConnection? _connection;

        public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
        {
            var connection = await GetConnectionAsync(cancellationToken);
            return await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }

        // The v7 client connects asynchronously, which DI singletons can't do at
        // construction time — so the connection is established lazily on first use
        // and reused (auto-recovery is on by default).
        private async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection is { IsOpen: true })
                return _connection;

            await _gate.WaitAsync(cancellationToken);
            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                var factory = new ConnectionFactory
                {
                    HostName = _options.Host,
                    Port = _options.Port,
                    VirtualHost = _options.VirtualHost,
                    UserName = _options.UserName,
                    Password = _options.Password,
                };

                _connection = await factory.CreateConnectionAsync(cancellationToken);
                return _connection;
            }
            finally
            {
                _gate.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
                await _connection.DisposeAsync();
            _gate.Dispose();
        }
    }
}
