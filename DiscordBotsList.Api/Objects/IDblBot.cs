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
        /// <summary>
        /// Represents the <see cref="DateTime"/> at which this <see cref="IDblBot"/> was approved.
        /// </summary>
        DateTime ApprovedAt { get; }

        /// <summary>
        /// Represents a collection of featured guild IDs on this <see cref="IDblBot"/>.
        /// </summary>
        IReadOnlyList<ulong> FeaturedGuildIds { get; }

		/// <summary>
		/// Represents the primary owner of this <see cref="IDblBot"/>.
		/// </summary>
		ulong PrimaryOwnerId { get; }

        /// <summary>
        /// Represents a collection of owners for this <see cref="IDblBot"/>.
        /// </summary>
        IReadOnlyList<ulong> OwnerIds { get; }

		/// <summary>
		/// Represents the Discord API library that was used for this bot.
		/// </summary>
		string LibraryUsed { get; }

		/// <summary>
		/// Represents the default prefix that this <see cref="IDblBot"/> uses.
		/// </summary>
		string PrefixUsed { get; }

		/// <summary>
		/// Represents the short description of this <see cref="IDblBot"/> (up to 140 characters).
		/// </summary>
		string ShortDescription { get; }

		/// <summary>
		/// Represents the full description of this <see cref="IDblBot"/> (may contain HTML and/or Markdown).
		/// </summary>
		string LongDescription { get; }

		/// <summary>
		/// Represents a collection of search tags that this <see cref="IDblBot"/> is paired to.
		/// </summary>
		IReadOnlyList<string> Tags { get; }

		/// <summary>
		/// Represents the website URL of this <see cref="IDblBot"/>.
		/// </summary>
		string WebsiteUrl { get; }

		/// <summary>
		/// Represents an invite URL to the support guild of this <see cref="IDblBot"/>.
		/// </summary>
		string SupportInviteUrl { get; }

		/// <summary>
		/// Represents the GitHub repository URL for this <see cref="IDblBot"/>.
		/// </summary>
		string GithubUrl { get; }

        string InviteUrl { get; }

        bool IsCertified { get; }

		string VanityUrl { get; }

        /// <summary>
		/// Represents the total vote count of this <see cref="IDblBot"/>.
		/// </summary>
		int Points { get; }

		/// <summary>
		/// Represents the monthly vote count of this <see cref="IDblBot"/>.
		/// </summary>
		int MonthlyPoints { get; }

		ulong? DonateBotGuildId { get; }

		/// <summary>
		/// Asynchronously retrieves the stats of this <see cref="IDblBot"/>. This can return null.
		/// </summary>
		/// <returns>An object that represents the stats for this <see cref="IDblBot"/>.</returns>
		Task<IDblBotStats> GetStatsAsync();
	}
}
