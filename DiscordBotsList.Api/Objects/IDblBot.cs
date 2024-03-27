using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Objects
{
    public interface IDblBot : IDblEntity
    {
        string PrefixUsed { get; }

        string ShortDescription { get; }

        string LongDescription { get; }

        List<string> Tags { get; }

        string WebsiteUrl { get; }

        string SupportUrl { get; }

        string GithubUrl { get; }

        List<ulong> OwnerIds { get; }

        string InviteUrl { get; }

        DateTime ApprovedAt { get; }

        bool IsCertified { get; }

        string VanityUrl { get; }

        int Points { get; }
        
        int MonthlyPoints { get; }

        Task<IDblBotStats> GetStatsAsync();
    }

    public interface IDblSelfBot : IDblBot
    {
        Task<List<IDblEntity>> GetVotersAsync();

        Task<bool> HasVotedAsync(ulong userId);

        Task<bool> IsWeekendAsync();

        Task UpdateStatsAsync(int guildCount);

        Task UpdateStatsAsync(int[] shards);

        Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards);
    }
}