using DiscordBotsList.Api.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class GuildCountObject
    {
        [JsonPropertyName("server_count")]
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
        internal int guildCount
        {
            get; set;
        }
    }

    internal class ShardedObject
    {
        [JsonPropertyName("shards")]
        internal int[] shards
        {
            get; set;
        }

        [JsonPropertyName("shard_count")]
        internal int shardCount
        {
            get; set;
        }
    }

    internal class ShardedGuildCountObject
    {
        [JsonPropertyName("shards")]
        public int[] Shards
        {
            get; set;
        }

        [JsonPropertyName("shard_id")]
        public int ShardId
        {
            get; set;
        }

        [JsonPropertyName("shard_count")]
        public int ShardCount
        {
            get; set;
        }

        [JsonPropertyName("server_count")]
        public int GuildCount
        {
            get; set;
        }
    }
}