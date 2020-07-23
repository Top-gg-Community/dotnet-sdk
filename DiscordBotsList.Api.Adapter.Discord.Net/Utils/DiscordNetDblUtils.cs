using Discord.WebSocket;

namespace DiscordBotsList.Api.Adapter.Discord.Net.Utils
{
	public static class DiscordNetDblUtils
	{
		/// <summary>
		/// Initializes a new <see cref="DiscordDblClient"/>.
		/// </summary>
		/// <param name="client">Represents the <see cref="DiscordSocketClient"/> to use when initializing this <see cref="DiscordDblClient"/>.</param>
		/// <param name="token"></param>
		/// <returns>A new instance of <see cref="DiscordDblClient"/>.</returns>
		public static DiscordDblClient CreateDblApi(this DiscordSocketClient client, string token)
			=> new DiscordDblClient(client, token);

		/// <summary>
		/// Initializes a new <see cref="DiscordShardedDblClient"/>.
		/// </summary>
		/// <param name="client">Represents the <see cref="DiscordShardedClient"/> to use when initializing this <see cref="DiscordShardedDblClient"/>.</param>
		/// <param name="token"></param>
		/// <returns>A new instance of <see cref="DiscordShardedDblClient"/>.</returns>
		public static DiscordShardedDblClient CreateDblApi(this DiscordShardedClient client, string token)
			=> new DiscordShardedDblClient(client, token);
	}
}
