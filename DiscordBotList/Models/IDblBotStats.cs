using System.Collections.Generic;

namespace DiscordBotList.Models
{
	/// <summary>
	/// Represents a generic container for guild statistics of an <see cref="IDblBot"/>.
	/// </summary>
	public interface IDblBotStats
	{
		/// <summary>
		/// Represents the total number of guilds that a bot is connected to.
		/// </summary>
		int GuildCount { get; }

		/// <summary>
		/// Represents a collection of guild counts in each shard.
		/// </summary>
		IReadOnlyList<int> Shards { get; }

		/// <summary>
		/// Represents the total number of shards that a bot is using.
		/// </summary>
		int ShardCount { get; }
	}
}
