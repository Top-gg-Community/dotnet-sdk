using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class BotStatsObject : ShardedObject, IDblBotStats
	{
		public int GuildCount => guildCount;

		public IReadOnlyList<int> Shards => shards.ToList();

		public int ShardCount => shardCount;

		[JsonProperty("server_count")]
		internal int guildCount;
	}
}
