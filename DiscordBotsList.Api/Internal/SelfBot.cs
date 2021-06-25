using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class SelfBot : Bot, IDblSelfBot
    {
        public async Task<List<IDblEntity>> GetVotersAsync()
            => await ((AuthDiscordBotListApi)api).GetVotersAsync();

        public async Task<bool> HasVotedAsync(ulong userId)
            => await ((AuthDiscordBotListApi)api).HasVoted(userId);

        public async Task<bool> IsWeekendAsync()
            => await ((AuthDiscordBotListApi)api).IsWeekendAsync();

        public async Task UpdateStatsAsync(int guildCount)
            => await ((AuthDiscordBotListApi)api).UpdateStats(guildCount);

        public async Task UpdateStatsAsync(int[] shards)
            => await ((AuthDiscordBotListApi)api).UpdateStats(0, shards.Length, shards);

        public async Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards)
            => await ((AuthDiscordBotListApi)api).UpdateStats(shardCount, totalShards, shards);
    }
}