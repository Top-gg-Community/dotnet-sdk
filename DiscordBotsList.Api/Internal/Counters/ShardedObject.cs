using Newtonsoft.Json;

namespace DiscordBotsList.Api.Internal
{
    internal class ShardedObject
	{
		[JsonProperty("shards")]
		internal int[] shards;

		[JsonProperty("shard_count")]
		internal int shardCount;
	}
}
