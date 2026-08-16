using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     A single metrics payload for a project. The fields you send depend on the
    ///     authenticated project's platform:
    ///     <list type="bullet">
    ///         <item><description>Discord bot: <see cref="ServerCount" /> and <see cref="ShardCount" />.</description></item>
    ///         <item><description>Discord server: <see cref="MemberCount" /> and <see cref="OnlineCount" />.</description></item>
    ///         <item><description>Roblox game: <see cref="PlayerCount" />.</description></item>
    ///         <item><description>Minecraft server: the <c>Players*</c>, <c>Tick*</c>, <c>Uptime*</c> and <c>WorldSize*</c> fields.</description></item>
    ///     </list>
    ///     Sending a shape that does not match the project's platform returns <c>422</c>.
    /// </summary>
    public class ProjectMetrics
    {
        /// <summary>Total number of servers the Discord bot is currently in.</summary>
        [JsonPropertyName("server_count")]
        public int? ServerCount { get; set; }

        /// <summary>Total number of shards the Discord bot is currently running.</summary>
        [JsonPropertyName("shard_count")]
        public int? ShardCount { get; set; }

        /// <summary>Total number of members in the Discord server.</summary>
        [JsonPropertyName("member_count")]
        public int? MemberCount { get; set; }

        /// <summary>Number of members currently online. Must not exceed <see cref="MemberCount" />.</summary>
        [JsonPropertyName("online_count")]
        public int? OnlineCount { get; set; }

        /// <summary>Current number of players in the Roblox game.</summary>
        [JsonPropertyName("player_count")]
        public int? PlayerCount { get; set; }

        /// <summary>Number of players currently online. Must not exceed <see cref="PlayersMax" />.</summary>
        [JsonPropertyName("players_online")]
        public int? PlayersOnline { get; set; }

        /// <summary>Maximum number of players the Minecraft server allows.</summary>
        [JsonPropertyName("players_max")]
        public int? PlayersMax { get; set; }

        /// <summary>Server uptime in seconds since the last restart.</summary>
        [JsonPropertyName("uptime_seconds")]
        public long? UptimeSeconds { get; set; }

        /// <summary>Current tick rate of the server. Typically close to 20.0 on a healthy server.</summary>
        [JsonPropertyName("tick_rate")]
        public double? TickRate { get; set; }

        /// <summary>Average duration of a single server tick, in milliseconds.</summary>
        [JsonPropertyName("tick_duration_avg")]
        public double? TickDurationAverageMilliseconds { get; set; }

        /// <summary>Total world size along the X axis, in blocks.</summary>
        [JsonPropertyName("world_size_x")]
        public int? WorldSizeX { get; set; }

        /// <summary>Total world size along the Y axis, in blocks.</summary>
        [JsonPropertyName("world_size_y")]
        public int? WorldSizeY { get; set; }

        /// <summary>Builds a payload for a Discord bot project.</summary>
        public static ProjectMetrics ForDiscordBot(int? serverCount = null, int? shardCount = null)
        {
            return new ProjectMetrics { ServerCount = serverCount, ShardCount = shardCount };
        }

        /// <summary>Builds a payload for a Discord server project.</summary>
        public static ProjectMetrics ForDiscordServer(int? memberCount = null, int? onlineCount = null)
        {
            return new ProjectMetrics { MemberCount = memberCount, OnlineCount = onlineCount };
        }

        /// <summary>Builds a payload for a Roblox game project.</summary>
        public static ProjectMetrics ForRobloxGame(int playerCount)
        {
            return new ProjectMetrics { PlayerCount = playerCount };
        }

        /// <summary>Builds a payload for a Minecraft server project.</summary>
        public static ProjectMetrics ForMinecraftServer(
            int? playersOnline = null,
            int? playersMax = null,
            long? uptimeSeconds = null,
            double? tickRate = null,
            double? tickDurationAverageMilliseconds = null,
            int? worldSizeX = null,
            int? worldSizeY = null)
        {
            return new ProjectMetrics
            {
                PlayersOnline = playersOnline,
                PlayersMax = playersMax,
                UptimeSeconds = uptimeSeconds,
                TickRate = tickRate,
                TickDurationAverageMilliseconds = tickDurationAverageMilliseconds,
                WorldSizeX = worldSizeX,
                WorldSizeY = worldSizeY,
            };
        }
    }

    /// <summary>A single entry in a metrics batch submission.</summary>
    public class MetricsBatchEntry
    {
        /// <summary>
        ///     When the metrics were collected. Must not be more than 5 minutes in the
        ///     future. Entries without a timestamp are applied first, in the order they
        ///     appear in the request; dated entries are applied afterwards in ascending
        ///     timestamp order.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>The metrics payload for this entry.</summary>
        [JsonPropertyName("metrics")]
        public ProjectMetrics Metrics { get; set; }
    }

    /// <summary>
    ///     Request body for <c>POST /projects/@me/metrics/batch</c>. Contains up to
    ///     100 entries.
    /// </summary>
    public class MetricsBatchRequest
    {
        /// <summary>The batch of metrics entries to submit.</summary>
        [JsonPropertyName("data")]
        public List<MetricsBatchEntry> Data { get; set; }
    }
}
