using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotList.Models
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
        /// Represents the vanity URL for this <see cref="IDblBot"/> (or a default page URL if a vanity is unspecified).
        /// </summary>
        string VanityUrl { get; }

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
		string GitHubUrl { get; }

		/// <summary>
		/// Represents the invite URL for this <see cref="IDblBot"/>.
		/// </summary>
        string InviteUrl { get; }

		/// <summary>
		/// Represents the certification status of this <see cref="IDblBot"/>.
		/// </summary>
        bool IsCertified { get; }

        /// <summary>
		/// Represents the total vote count of this <see cref="IDblBot"/>.
		/// </summary>
		int Points { get; }

		/// <summary>
		/// Represents the monthly vote count of this <see cref="IDblBot"/>.
		/// </summary>
		int MonthlyPoints { get; }

		/// <summary>
		/// Represents the guild that DonateBot will be applied to for this <see cref="IDblBot"/>.
		/// </summary>
		/// <remarks>
		/// For more information, refer to https://top.gg/donatebot.
		/// </remarks>
		ulong? DonateBotGuildId { get; }

		/// <summary>
		/// Asynchronously retrieves the stats of this <see cref="IDblBot"/> (nullable).
		/// </summary>
		/// <returns>An <see cref="IDblBotStats"/> object that represents the stats for this <see cref="IDblBot"/>.</returns>
		Task<IDblBotStats> GetStatsAsync();
	}
}
