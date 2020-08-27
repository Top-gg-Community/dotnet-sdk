using Newtonsoft.Json;
using DiscordBotList.Models;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblEntity"/>
	internal class DblEntity : IDblEntity
    {
        /// <inheritdoc />
		[JsonProperty("id")]
		public ulong Id { get; internal set; }

        /// <inheritdoc />
		[JsonProperty("username")]
		public string Username { get; internal set; }

        /// <inheritdoc />
		[JsonProperty("discrminator")]
        public string Discriminator { get; internal set; }

		[JsonProperty("avatar")]
		internal string AvatarId { get; set; }

        /// <inheritdoc />
		[JsonProperty("defAvatar")]
        public string DefaultAvatarHash { get; internal set; }

        /// <inheritdoc />
		public string AvatarUrl => DblApi.GetAvatarUrl(Id, AvatarId, Discriminator);

        /// <summary>
		/// Returns a string that represents the fully-typed reference name for this <see cref="IDblEntity"/>.
		/// </summary>
		public override string ToString()
		    => $"{Username}#{Discriminator}";
	}
}
