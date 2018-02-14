using System.Collections.Generic;

namespace DiscordBotsList.Api
{
	public interface IDblBotStats
	{
		int GuildCount { get; }

		IReadOnlyList<int> Shards { get; }

		int ShardCount { get; }
	}
}