using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotsList.Api.Internal
{
    public class Entity : IEntity
    {
		public ulong Id => id;

		public string Username => username;

		public string Discriminator => discriminator;

		public string AvatarUrl => avatarUrl;

		[JsonProperty("id")]
		internal ulong id;

		[JsonProperty("username")]
		internal string username;

		[JsonProperty("discrminator")]
		internal string discriminator;

		[JsonProperty("avatar")]
		internal string avatarUrl;

		[JsonProperty("defAvatar")]
		internal string defAvatarUrl;

		public override string ToString()
		{
			return $"{username}#{discriminator}";
		}
	}
}
