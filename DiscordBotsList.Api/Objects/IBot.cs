using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    public interface IBot : IEntity
    {
		string LibraryUsed { get; }

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

		int Score { get; }

		Task<IBotStats> GetStatsAsync();
	}

	public interface ISelfBot : IBot
	{
		Task<List<IEntity>> GetVotersAsync(int? days = null);
		Task<List<ulong>> GetVoterIdsAsync(int? days = null);

		Task UpdateStatsAsync(int guildCount);
		Task UpdateStatsAsync(int[] shards);
		Task UpdateStatsAsync(int shardCount, int totalShards, params int[] shards);
	}
}
