using Discord;
using Discord.WebSocket;
using DiscordBotsList.Api.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Adapter.Discord.Net
{
    /// <summary>
    ///     Creates a <see cref="DiscordNetTopggV1Api" /> backed by the current Top.gg v1
    ///     API. Pass a v1 (Bearer) token from your project's Integrations &amp; API
    ///     settings on Top.gg.
    /// </summary>
    public static class DiscordNetTopggV1Utils
    {
        public static DiscordNetTopggV1Api CreateTopggV1Api(this DiscordSocketClient client, string token)
        {
            return new DiscordNetTopggV1Api(client, token);
        }

        public static DiscordNetTopggV1Api CreateTopggV1Api(this DiscordShardedClient client, string token)
        {
            return new DiscordNetTopggV1Api(client, token);
        }
    }

    /// <summary>
    ///     Adapter that posts your bot's server (and shard) count to the Top.gg v1 API
    ///     (<c>PATCH /projects/@me/metrics</c>). Only applicable to Discord bot projects.
    /// </summary>
    public class DiscordNetTopggV1Api : IDisposable
    {
        private readonly TopggV1Client _api;
        private readonly IDiscordClient _client;

        public DiscordNetTopggV1Api(IDiscordClient client, string token)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _api = new TopggV1Client(token);
        }

        /// <summary>
        ///     Creates an <see cref="IAdapter" /> that submits your server count on
        ///     <c>RunAsync()</c>.
        /// </summary>
        /// <param name="updateTime">
        ///     The interval between submissions. Leave <c>null</c> to submit on every
        ///     invocation.
        /// </param>
        public IAdapter CreateListener(TimeSpan? updateTime = null)
        {
            return new TopggV1SubmissionAdapter(_api, _client, updateTime ?? TimeSpan.Zero);
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }

    internal class TopggV1SubmissionAdapter : IAdapter
    {
        private readonly TopggV1Client _api;
        private readonly IDiscordClient _client;
        private readonly TimeSpan _updateTime;

        private DateTime _lastTimeUpdated;
        private volatile bool _stopped;

        public TopggV1SubmissionAdapter(TopggV1Client api, IDiscordClient client, TimeSpan updateTime)
        {
            _api = api;
            _client = client;
            _updateTime = updateTime;
        }

        public event Action<string> Log;

        public virtual async Task RunAsync()
        {
            if (_stopped || DateTime.UtcNow <= _lastTimeUpdated + _updateTime)
            {
                return;
            }

            var metrics = new ProjectMetrics();

            if (_client is DiscordShardedClient sharded)
            {
                metrics.ServerCount = sharded.Shards.Sum(shard => shard.Guilds.Count);
                metrics.ShardCount = sharded.Shards.Count;
            }
            else
            {
                metrics.ServerCount = (await _client.GetGuildsAsync()).Count;
            }

            await _api.UpdateMetricsAsync(metrics);

            _lastTimeUpdated = DateTime.UtcNow;
            SendLog("Submitted stats to Top.gg!");
        }

        public virtual void Start()
        {
            _stopped = false;
        }

        public virtual void Stop()
        {
            _stopped = true;
        }

        protected void SendLog(string message)
        {
            Log?.Invoke(message);
        }
    }
}
