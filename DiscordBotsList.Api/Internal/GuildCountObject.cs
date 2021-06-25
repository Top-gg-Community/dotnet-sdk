using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class GuildCountObject
    {
        [JsonProperty("server_count")]
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

        [JsonProperty("server_count")]
        internal int guildCount;
    }

    internal class ShardedObject
    {
        [JsonProperty("shards")]
        internal int[] shards;

        [JsonProperty("shard_count")]
        internal int shardCount;
    }

    internal class ShardedGuildCountObject
    {
        [JsonProperty("shards")]
        public int[] Shards;

        [JsonProperty("shard_id")]
        public int ShardId;

        [JsonProperty("shard_count")]
        public int ShardCount;

        [JsonProperty("server_count")]
        public int GuildCount;
    }
}