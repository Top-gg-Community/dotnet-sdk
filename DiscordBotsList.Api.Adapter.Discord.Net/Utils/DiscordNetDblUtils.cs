using Discord.WebSocket;
using DiscordBotsList.Api.Extensions.DiscordNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api.Adapter.DiscordNet
{
	public static class DiscordNetDblUtils
	{
		/// <summary>
		/// Creates a DiscordBotsList Api
		/// </summary>
		/// <param name="client">your client</param>
		/// <param name="dblToken">Your DiscordBotsList token</param>
		/// <returns>A new instance of a DblApi</returns>
		public static DiscordNetDblApi CreateDblApi(this DiscordSocketClient client, string dblToken)
			=> new DiscordNetDblApi(client, dblToken);

		/// <summary>
		/// Creates a DiscordBotsList Api
		/// </summary>
		/// <param name="client">your client</param>
		/// <param name="dblToken">Your DiscordBotsList token</param>
		/// <returns>A new instance of a DblApi</returns>
		public static ShardedDiscordNetDblApi CreateDblApi(this DiscordShardedClient client, string dblToken)
			=> new ShardedDiscordNetDblApi(client, dblToken);
	}
}
