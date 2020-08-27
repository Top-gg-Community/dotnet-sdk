using Discord;
using Discord.WebSocket;

namespace DiscordBotList.Adapters.DNet
{
	/// <summary>
	/// Represents a factory for a <see cref="DiscordDblClient"/>.
	/// </summary>
	public static class DiscordDblFactory
	{
		/// <summary>
		/// Initializes a new <see cref="DiscordDblClient"/>.
		/// </summary>
		/// <param name="client">Represents the <see cref="DiscordSocketClient"/> to use when initializing this <see cref="DiscordDblClient"/>.</param>
		/// <param name="token">The token to use for the Discord Bot List API.</param>
		/// <returns>A new <see cref="DiscordDblClient"/> instance.</returns>
		public static DiscordDblClient CreateApiClient(this IDiscordClient client, string token)
			=> new DiscordDblClient(client, token);
    }
}
