using System.Collections.Generic;

namespace DiscordBotsList.Api.Objects
{
    public interface IDblBotStats
    {
        int GuildCount { get;}

        IReadOnlyList<int> Shards { get;}

        int ShardCount { get; }
    }
}