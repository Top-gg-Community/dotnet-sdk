using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api
{
	public class SocialConnections
	{
		[JsonProperty("youtube")]
		public string YouTubeChannelId { get; internal set; }

		[JsonProperty("reddit")]
		public string RedditName { get; internal set; }

		[JsonProperty("twitter")]
		public string TwitterName { get; internal set; }

		[JsonProperty("instagram")]
		public string InstagramName { get; internal set; }

		[JsonProperty("github")]
		public string GitHubName { get; internal set; }
	}
}
