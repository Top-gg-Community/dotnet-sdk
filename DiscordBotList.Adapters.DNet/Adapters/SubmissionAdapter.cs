using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordBotList.Adapters.DNet
{
    /// <summary>
    /// Represents an asynchronous submission adapter for an <see cref="IDiscordClient"/>.
    /// </summary>
    internal class SubmissionAdapter : IAdapter
    {
        /// <summary>
        /// Initializes a new <see cref="SubmissionAdapter"/>.
        /// </summary>
        /// <param name="apiClient">The <see cref="DblClient"/> for this <see cref="SubmissionAdapter"/> to use.</param>
        /// <param name="client">The <see cref="BaseDiscordClient"/> for this <see cref="SubmissionAdapter"/> to use.</param>
        /// <param name="properties">The properties of this <see cref="SubmissionAdapter"/> (<see cref="AdapterProperties.RunMode"/> is omitted).</param>
        public SubmissionAdapter(DblClient apiClient, IDiscordClient client, AdapterProperties properties = null)
        {
            if (apiClient.CurrentUserId != client.CurrentUser.Id)
                throw new ArgumentException("The DblClient must reference the specified current user in the IDiscordClient");

            ApiClient = apiClient;
            Client = client;
            properties = properties ?? AdapterProperties.Default;

            Interval = properties.Interval;
            Message = properties.Message;
            Timer = new Timer(UpdateInternal, this, Interval, Interval);
        }

        /// <summary>
        /// Fired when this <see cref="SubmissionAdapter"/> successfully updates.
        /// </summary>
        public event Action<string> AdapterUpdated;

        /// <inheritdoc />
        public DateTime LastUpdated { get; protected set; }

        protected DblClient ApiClient { get; }

        protected IDiscordClient Client { get; }

        protected TimeSpan Interval { get; }

        protected string Message { get; }

        private Timer Timer { get; set; }

        private bool Stopped { get; set; }

        /// <summary>
        /// Represents the disposal state of this <see cref="SubmissionAdapter"/>.
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

        /// <inheritdoc />
        public virtual void Start()
        {
            if (!Stopped || Disposed)
                return;

            Timer = CreateTimer();
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            if (Stopped || Disposed)
                return;

            Timer?.Dispose();
            Stopped = true;
        }

        /// <summary>
        /// Disposes of this <see cref="SubmissionAdapter"/>.
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
                await ApiClient.UpdateStatsAsync((await Client.GetGuildsAsync()).Count);
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
