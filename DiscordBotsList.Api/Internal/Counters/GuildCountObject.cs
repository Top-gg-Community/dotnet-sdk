using Newtonsoft.Json;

namespace DiscordBotsList.Api.Internal
{
	internal class GuildCountObject
	{
		[JsonProperty("server_count")]
		internal int guildCount;

		public GuildCountObject(int count)
		{
			guildCount = count;
		}
	}
}
