using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBotList.Models;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblSelfBot" />
	internal class DblSelfBot : DblBot, IDblSelfBot
	{
        /// <inheritdoc />
		public async Task<List<IDblEntity>> GetVotersAsync()
			=> await ((DblClient) Client).GetVotersAsync();

        /// <inheritdoc />
		public async Task<bool> HasVotedAsync(ulong userId)
	        => await ((DblClient) Client).HasVotedAsync(userId);

        /// <inheritdoc />
		public async Task<bool> IsWeekendAsync()
	        => await Client.IsWeekendAsync();

		/// <inheritdoc />
		public async Task UpdateStatsAsync(int guildCount)
			=> await ((DblClient) Client).UpdateStatsAsync(guildCount);

        /// <inheritdoc />
		public async Task UpdateStatsAsync(int[] shards)
			=> await ((DblClient) Client).UpdateStatsAsync(0, shards.Length, shards);

        /// <inheritdoc />
		public async Task UpdateStatsAsync(int startIndex, int shardCount, params int[] shards)
			=> await ((DblClient) Client).UpdateStatsAsync(startIndex, shardCount, shards);

        /// <inheritdoc />
		public async Task UpdateStatsAsync(int guildCount, int shardCount)
            => await ((DblClient) Client).UpdateStatsAsync(guildCount, shardCount);
    }
}
