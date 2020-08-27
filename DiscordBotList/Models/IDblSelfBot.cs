using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotList.Models
{
	/// <summary>
	/// Represents a generic client bot for the Discord Bot List API.
	/// </summary>
    public interface IDblSelfBot : IDblBot
	{
		/// <summary>
		/// Returns a list of all users that have voted for your bot for up to 1,000 entries. If you have more than 1,000, you are required to use webhooks instead.
		/// </summary>
		/// <returns>A list of all users that have voted.</returns>
		Task<List<IDblEntity>> GetVotersAsync();

		/// <summary>
		/// Returns a bool that defines if the specified user has voted for this bot within the last 12 hours.
		/// </summary>
		/// <param name="userId">Represents the ID of the user that you wish to check.</param>
		/// <returns>A bool that defines if a specific user has voted.</returns>
		Task<bool> HasVotedAsync(ulong userId);

		/// <summary>
		/// Returns a bool that defines if it is currently the weekend. If this returns true, the voting multiplier is doubled.
		/// </summary>
		/// <returns>A boolean value that defines if it is currently the weekend.</returns>
		Task<bool> IsWeekendAsync();

		/// <summary>
		/// Sends a request to update your stats by guild count.
		/// </summary>
		/// <param name="guildCount">Represents the current guild count for the bot.</param>
		Task UpdateStatsAsync(int guildCount);

		/// <summary>
		/// Sends a request to update your stats by individual shard values.
		/// </summary>
		/// <param name="shards">Represents an array of guild counts for shards.</param>
		Task UpdateStatsAsync(int[] shards);

		/// <summary>
		/// Sends a request to update your stats by specific shard values.
		/// </summary>
		/// <param name="startIndex">Represents the starting index at which to update your shards from.</param>
		/// <param name="shardCount">Represents the current shard count that the bot uses.</param>
		/// <param name="shards">Represents an array of guild counts for each shard, starting from the specified index.</param>
		Task UpdateStatsAsync(int startIndex, int shardCount, params int[] shards);

		/// <summary>
		/// Sends a request to update your stats by guild and shard count.
		/// </summary>
		/// <param name="guildCount">Represents the current guild count for the bot.</param>
		/// <param name="shardCount">Represents the current shard count that the bot uses.</param>
		Task UpdateStatsAsync(int guildCount, int shardCount);
	}
}
