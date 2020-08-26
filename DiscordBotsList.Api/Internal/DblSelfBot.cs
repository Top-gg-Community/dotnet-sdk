using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
	internal class DblSelfBot : DblBot, IDblSelfBot
	{
		public async Task<List<IDblEntity>> GetVotersAsync()
			=> await ((DblClient) Client).GetVotersAsync();

	    public async Task<bool> HasVotedAsync(ulong userId)
	        => await ((DblClient) Client).HasVotedAsync(userId);

	    public async Task<bool> IsWeekendAsync()
	        => await ((DblClient) Client).IsWeekendAsync();

		public async Task UpdateStatsAsync(int guildCount)
			=> await ((DblClient) Client).UpdateStatsAsync(guildCount);

		public async Task UpdateStatsAsync(int[] shards)
			=> await ((DblClient) Client).UpdateStatsAsync(0, shards.Length, shards);

		public async Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards)
			=> await ((DblClient) Client).UpdateStatsAsync(shardCount, totalShards, shards);

        public async Task UpdateStatsAsync(int guildCount, int shardCount)
            => await ((DblClient) Client).UpdateStatsAsync(guildCount, shardCount);
    }
}
