namespace DiscordBotsList.Api.Objects
{
	/// <summary>
	/// Represents a generic entity for the Discord Bot List API.
	/// </summary>
	public interface IDblEntity
	{
		/// <summary>
		/// Represents the ID of this <see cref="IDblEntity"/>.
		/// </summary>
		ulong Id { get; }

		/// <summary>
		/// Represents the username of this <see cref="IDblEntity"/>.
		/// </summary>
		string Username { get; }

		/// <summary>
		/// Represents the discriminator for this <see cref="IDblEntity"/>, which is located after the username.
		/// </summary>
		string Discriminator { get; }
		
		/// <summary>
		/// Represents the avatar URL for this <see cref="IDblEntity"/>, or a default avatar URL if none could be found.
		/// </summary>
		string AvatarUrl { get; }
	}
}
