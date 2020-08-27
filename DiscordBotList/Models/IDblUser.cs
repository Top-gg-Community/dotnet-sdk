namespace DiscordBotList.Models
{
	/// <summary>
	/// Represents a generic user for the Discord Bot List API.
	/// </summary>
	public interface IDblUser : IDblEntity
    {
        /// <summary>
		/// Represents the supporter status of this <see cref="IDblUser"/>.
		/// </summary>
		bool IsSupporter { get; }

		/// <summary>
		/// Represents the certification status of this <see cref="IDblUser"/>.
		/// </summary>
		bool IsCertified { get; }

		/// <summary>
		/// Represents the moderation status of this <see cref="IDblUser"/>.
		/// </summary>
		bool IsModerator { get; }

		/// <summary>
		/// Represents the web moderation status of this <see cref="IDblUser"/>.
		/// </summary>
		bool IsWebModerator { get; }

		/// <summary>
		/// Represents the administrator status of this <see cref="IDblUser"/>.
		/// </summary>
		bool IsAdmin { get; }

        /// <summary>
        /// Represents the status of this <see cref="IDblUser"/>.
        /// </summary>
        DblStatus Status { get; }

		/// <summary>
		/// Represents a collection of social media connections that this <see cref="IDblUser"/> has specified.
		/// </summary>
        SocialConnections Connections { get; }

		/// <summary>
		/// Represents the short description that this <see cref="IDblUser"/> has applied.
		/// </summary>
		string Biography { get; }

		/// <summary>
		/// Represents the page URL for this <see cref="IDblUser"/>.
		/// </summary>
        string PageUrl { get; }

		/// <summary>
		/// Represents the image URL of the banner that this <see cref="IDblUser"/> uses.
		/// </summary>
		string BannerUrl { get; }

        /// <summary>
		/// Represents the color hex code that this <see cref="IDblUser"/> uses.
		/// </summary>
		string ColorHex { get; }

		/// <summary>
		/// Represents the raw color value that this <see cref="IDblUser"/> uses.
		/// </summary>
		int RawColor { get; }
	}
}
