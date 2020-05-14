using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Objects
{
	/// <summary>
	/// Represents a generic bot for the Discord Bot List API.
	/// </summary>
	public interface IDblBot : IDblEntity
    {
		string LibraryUsed { get; }

		string PrefixUsed { get; }

		/// <summary>
		/// Represents the short description used for this bot (up to 140 characters).
		/// </summary>
		string ShortDescription { get; }

		/// <summary>
		/// Represents the full description used for this bot.
		/// </summary>
		string LongDescription { get; }

		List<string> Tags { get; }

		string WebsiteUrl { get; }

		string SupportUrl { get; }

		string GithubUrl { get; }

		List<ulong> OwnerIds { get; }

		string InviteUrl { get; }

		/// <summary>
		/// Represents the <see cref="DateTime"/> at which this bot was approved.
		/// </summary>
		DateTime ApprovedAt { get; }

		bool IsCertified { get; }

		string VanityUrl { get; }

		int Points { get; }

		/// <summary>
		/// Returns the stats of this bot. If no results were found, this returns null.
		/// </summary>
		/// <returns>An object that represents the stats for this bot.</returns>
		Task<IDblBotStats> GetStatsAsync();
	}
}
