using System.Collections.Generic;
using System.Linq;
using DiscordBotList.Models;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblBotStats" />
    internal class DblBotStats : StatsObject, IDblBotStats
	{
        IReadOnlyList<int> IDblBotStats.Shards => Shards.ToList();

        int IDblBotStats.ShardCount => ShardCount;

        int IDblBotStats.GuildCount => GuildCount;
    }
}
