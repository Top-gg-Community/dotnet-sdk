using Newtonsoft.Json;

namespace DiscordBotsList.Api.Internal
{
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
