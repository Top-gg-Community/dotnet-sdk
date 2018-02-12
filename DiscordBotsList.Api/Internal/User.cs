using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api.Internal
{
	internal class User : Entity, IUser
	{
		[JsonProperty("bio")]
		internal string biography;

		[JsonProperty("banner")]
		internal string bannerUrl;

		[JsonProperty("social")]
		internal SocialConnections social;

		[JsonProperty("color")]
		internal string color;

		[JsonProperty("supporter")]
		internal bool isSupporter;

		[JsonProperty("certifiedDev")]
		internal bool isCertified;

		[JsonProperty("mod")]
		internal bool isModerator;

		[JsonProperty("webMod")]
		internal bool isWebModerator;

		[JsonProperty("admin")]
		internal bool isAdmin;

		public string Biography => biography;
		public string BannerUrl => bannerUrl;
		public SocialConnections Connections => social;
		public string Color => color;
		public bool IsSupporter => isSupporter;
		public bool IsCertified => isCertified;
		public bool IsModerator => isModerator;
		public bool IsWebModerator => isWebModerator;
		public bool IsAdmin => isAdmin;
	}
}
