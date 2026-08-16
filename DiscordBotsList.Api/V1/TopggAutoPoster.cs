using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Posts project metrics to the Top.gg v1 API on a fixed interval. Works for any
    ///     project type: supply a provider that builds the <see cref="ProjectMetrics" />
    ///     for your app (Discord bot, game server, and so on) and the poster handles the
    ///     rest. Framework-agnostic, so it fits plain console apps, Discord bots and game
    ///     servers alike.
    /// </summary>
    public sealed class TopggAutoPoster : IDisposable
    {
        /// <summary>The default interval between posts (30 minutes, same as the other official SDKs).</summary>
        public static readonly TimeSpan DefaultInterval = TimeSpan.FromMinutes(30);

        private readonly TopggV1Client _api;
        private readonly Func<Task<ProjectMetrics>> _metricsProvider;
        private readonly TimeSpan _interval;
        private readonly SemaphoreSlim _postLock = new(1, 1);
        private Timer _timer;
        private volatile bool _started;

        public TopggAutoPoster(TopggV1Client api, Func<Task<ProjectMetrics>> metricsProvider, TimeSpan? interval = null)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _metricsProvider = metricsProvider ?? throw new ArgumentNullException(nameof(metricsProvider));

            _interval = interval ?? DefaultInterval;
            if (_interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be positive.");
            }
        }

        /// <summary>Raised after a successful post with the metrics that were sent.</summary>
        public event Func<ProjectMetrics, Task> Posted;

        /// <summary>Raised when a post fails. A failed post is retried on the next tick.</summary>
        public event Func<Exception, Task> PostFailed;

        /// <summary>Whether the poster is running.</summary>
        public bool IsRunning => _started;

        /// <summary>
        ///     Starts posting immediately and then on every interval. Calling this when
        ///     already running has no effect.
        /// </summary>
        public void Start()
        {
            if (_started)
            {
                return;
            }

            _started = true;
            _timer = new Timer(_ => PostAndSwallowExceptions(), null, TimeSpan.Zero, _interval);
        }

        /// <summary>Stops the poster. The current post, if any, is allowed to finish.</summary>
        public void Stop()
        {
            _started = false;
            _timer?.Dispose();
            _timer = null;
        }

        /// <summary>Posts the current metrics right now, regardless of the interval.</summary>
        public async Task PostAsync(CancellationToken cancellationToken = default)
        {
            // Skip overlapping ticks instead of queuing them.
            if (!await _postLock.WaitAsync(0, cancellationToken))
            {
                return;
            }

            try
            {
                var metrics = await _metricsProvider();
                await _api.UpdateMetricsAsync(metrics, cancellationToken);

                var posted = Posted;
                if (posted != null)
                {
                    await posted(metrics);
                }
            }
            catch (Exception ex)
            {
                var failed = PostFailed;
                if (failed != null)
                {
                    await failed(ex);
                }
            }
            finally
            {
                _postLock.Release();
            }
        }

        public void Dispose()
        {
            Stop();
            _postLock.Dispose();
        }

        private void PostAndSwallowExceptions()
        {
            try
            {
                PostAsync().GetAwaiter().GetResult();
            }
            catch
            {
                // PostAsync surfaces failures through the PostFailed event; never let a
                // timer callback throw.
            }
        }
    }
}
