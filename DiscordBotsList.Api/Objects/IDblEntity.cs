namespace DiscordBotsList.Api.Objects
{
	/// <summary>
	/// Represents a generic entity for the Discord Bot List API.
	/// </summary>
	public interface IDblEntity
	{
		/// <summary>
		/// Represents the Discord Snowflake ID of this <see cref="IDblEntity"/>.
		/// </summary>
		ulong Id { get; }

		/// <summary>
		/// Represents the username of this <see cref="IDblEntity"/>.
		/// </summary>
		string Username { get; }

		/// <summary>
		/// Represents the discriminator of this <see cref="IDblEntity"/>.
		/// </summary>
		string Discriminator { get; }

        /// <summary>
		/// Represents the avatar URL of this <see cref="IDblEntity"/> (returns the default URL if unspecified).
		/// </summary>
		string AvatarUrl { get; }
	}
}
