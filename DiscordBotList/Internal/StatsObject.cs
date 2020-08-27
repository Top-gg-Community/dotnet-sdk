using Newtonsoft.Json;

namespace DiscordBotList.Internal
{
    internal class StatsObject
	{
		[JsonProperty("shards")]
		public int[] Shards { get; set; }

		[JsonProperty("shard_id")]
		public int ShardId { get; set; }

		[JsonProperty("shard_count")]
		public int ShardCount { get; set; }

		[JsonProperty("server_count")]
		public int GuildCount { get; set; }
	}
}
