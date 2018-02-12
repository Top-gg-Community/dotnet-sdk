using System.Collections.Generic;

namespace DiscordBotsList.Api
{
	public interface IBotStats
	{
		int GuildCount { get; }

		IReadOnlyList<int> Shards { get; }

		int ShardCount { get; }
	}
}