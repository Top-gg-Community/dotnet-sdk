using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
	internal class SelfBot : Bot, IDblSelfBot
	{
		public async Task<List<IDblEntity>> GetVotersAsync()
			=> await ((DblClient) api).GetVotersAsync();

	    public async Task<bool> HasVotedAsync(ulong userId)
	        => await ((DblClient) api).HasVotedAsync(userId);

	    public async Task<bool> IsWeekendAsync()
	        => await ((DblClient) api).IsWeekendAsync();

		public async Task UpdateStatsAsync(int guildCount)
			=> await ((DblClient) api).UpdateStatsAsync(guildCount);

		public async Task UpdateStatsAsync(int[] shards)
			=> await ((DblClient) api).UpdateStatsAsync(0, shards.Length, shards);

		public async Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards)
			=> await ((DblClient) api).UpdateStatsAsync(shardCount, totalShards, shards);
	}
}
