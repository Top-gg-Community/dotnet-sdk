using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api.Internal
{
	internal class User : Entity, IUser
	{
		[JsonProperty("bio")]
		public string Biography;

		[JsonProperty("banner")]
		public string BannerUrl;

		[JsonProperty("social")]
		public SocialConnections Social;

		[JsonProperty("color")]
		public string Color;

		[JsonProperty("supporter")]
		public bool IsSupporter;

		[JsonProperty("certifiedDev")]
		public bool IsCertified;

		[JsonProperty("mod")]
		public bool IsModerator;

		[JsonProperty("webMod")]
		public bool IsWebModerator;

		[JsonProperty("admin")]
		public bool IsAdmin;
	}
}
