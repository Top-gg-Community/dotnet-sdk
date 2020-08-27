using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordBotList.Adapters.DNet
{
    /// <summary>
    /// Represents a stats submission adapter for a <see cref="BaseSocketClient"/>.
    /// </summary>
    internal class SocketSubmissionAdapter : IAdapter
    {
        /// <summary>
        /// Initializes a new <see cref="SocketSubmissionAdapter"/>.
        /// </summary>
        /// <param name="apiClient">The <see cref="DblClient"/> for this <see cref="SocketSubmissionAdapter"/> to use.</param>
        /// <param name="client">The <see cref="BaseDiscordClient"/> for this <see cref="SocketSubmissionAdapter"/> to use.</param>
        /// <param name="properties">The properties of this <see cref="SocketSubmissionAdapter"/>.</param>
        public SocketSubmissionAdapter(DblClient apiClient, BaseSocketClient client, AdapterProperties properties = null)
        {
            if (apiClient.CurrentUserId != client.CurrentUser.Id)
                throw new ArgumentException("The DblClient must reference the specified current user in the IDiscordClient");

            ApiClient = apiClient;
            Client = client;
            properties = properties ?? AdapterProperties.Default;

            Interval = properties.Interval;
            RunMode = properties.RunMode;
            Message = properties.Message;

            if (RunMode == AdapterRunMode.Async)
            {
                Timer = new Timer(UpdateInternal, this,  Interval, Interval);
            }
            else
            {
                Client.JoinedGuild += UpdateAsync;
                Client.LeftGuild += UpdateAsync;
            }
        }

        /// <summary>
        /// Fired when this <see cref="SocketSubmissionAdapter"/> successfully submits a count update.
        /// </summary>
        public event Action<string> AdapterUpdated;

        /// <inheritdoc />
        public DateTime LastUpdated { get; protected set; }

        protected DblClient ApiClient { get; }

        protected BaseSocketClient Client { get; }

        protected TimeSpan Interval { get; }

        protected AdapterRunMode RunMode { get; }

        protected string Message { get; }

        private Timer Timer { get; set; }

        private bool Stopped { get; set; }

        /// <summary>
        /// Represents the disposal state of this <see cref="SocketSubmissionAdapter"/>.
        /// </summary>
        public bool Disposed { get; protected set; }

        /// <inheritdoc />
        public virtual async Task UpdateAsync()
        {
            if (Stopped)
                return;

            if (DateTime.UtcNow < LastUpdated.Add(Interval))
                return;

            await UpdateClientStatsAsync();
            LastUpdated = DateTime.UtcNow;
            InvokeUpdatedEvent(Message);
        }

        private async Task UpdateAsync(SocketGuild guild)
            => await UpdateAsync();

        /// <inheritdoc />
        public virtual void Start()
        {
            if (!Stopped || Disposed)
                return;

            if (RunMode == AdapterRunMode.Sync)
            {
                Client.JoinedGuild += UpdateAsync;
                Client.LeftGuild += UpdateAsync;
                return;
            }

            Timer = CreateTimer();
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            if (Stopped || Disposed)
                return;

            if (RunMode == AdapterRunMode.Sync)
            {
                Client.JoinedGuild -= UpdateAsync;
                Client.LeftGuild -= UpdateAsync;
                return;
            }

            Timer?.Dispose();
            Stopped = true;
        }

        /// <summary>
        /// Disposes of this <see cref="SocketSubmissionAdapter"/>.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            Timer?.Dispose();
            Disposed = true;
        }

        protected void InvokeUpdatedEvent(string message)
            => AdapterUpdated?.Invoke(message);

        private async Task UpdateClientStatsAsync()
        {
            if (Client is DiscordShardedClient shardClient)
            {
                await ApiClient.UpdateStatsAsync(shardClient.Shards.Select(x => x.Guilds.Count).ToArray());
            }
            else
            {
                await ApiClient.UpdateStatsAsync(Client.Guilds.Count);
            }
        }

        private Timer CreateTimer()
            => new Timer(UpdateInternal, this, Interval, Interval);

        private static void UpdateInternal(object state)
        {
            if (!(state is IAdapter adapter))
                return;

            adapter.UpdateAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
