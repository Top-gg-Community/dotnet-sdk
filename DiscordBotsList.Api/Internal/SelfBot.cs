using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Internal
{
	internal class SelfBot : Bot, IDblSelfBot
	{
		public async Task<List<ulong>> GetVoterIdsAsync(int? days = null)
			=> await (api as AuthDiscordBotListApi).GetVoterIdsAsync(days);

		public async Task<List<IDblEntity>> GetVotersAsync(int? days = null)
			=> await (api as AuthDiscordBotListApi).GetVotersAsync(days);

		public async Task UpdateStatsAsync(int guildCount)
			=> await (api as AuthDiscordBotListApi).UpdateStats(guildCount);

		public async Task UpdateStatsAsync(int[] shards)
			=> await (api as AuthDiscordBotListApi).UpdateStats(0, shards.Length, shards);

		public async Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards)
			=> await (api as AuthDiscordBotListApi).UpdateStats(shardCount, totalShards, shards);
	}
}
