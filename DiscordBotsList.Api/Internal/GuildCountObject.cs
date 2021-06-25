using DiscordBotsList.Api.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class GuildCountObject
    {
        [JsonPropertyName("server_count")]
        [JsonInclude]
        internal int guildCount;

        public GuildCountObject(int count)
        {
            guildCount = count;
        }
    }

    internal class BotStatsObject : ShardedObject, IDblBotStats
    {
        public int GuildCount => guildCount;

        public IReadOnlyList<int> Shards => shards.ToList();

        public int ShardCount => shardCount;

        [JsonPropertyName("server_count")]
        [JsonInclude]
        internal int guildCount;
    }

    internal class ShardedObject
    {
        [JsonPropertyName("shards")]
        [JsonInclude]
        internal int[] shards;

        [JsonPropertyName("shard_count")]
        [JsonInclude]
        internal int shardCount;
    }

    internal class ShardedGuildCountObject
    {
        [JsonPropertyName("shards")]
        [JsonInclude]
        public int[] Shards;

        [JsonPropertyName("shard_id")]
        [JsonInclude]
        public int ShardId;

        [JsonPropertyName("shard_count")]
        [JsonInclude]
        public int ShardCount;

        [JsonPropertyName("server_count")]
        [JsonInclude]
        public int GuildCount;
    }
}