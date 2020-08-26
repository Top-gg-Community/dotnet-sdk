using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class DblBot : DblEntity, IDblBot
	{
		internal BaseDblClient Client;

        [JsonProperty("support")]
		internal string SupportInviteCode { get; }

        [JsonProperty("date")]
		public DateTime ApprovedAt { get; }

		[JsonProperty("github")]
        public string GithubUrl { get; }

        [JsonProperty("invite")]
        internal string CustomInviteUrl { get; }

		public string InviteUrl => CustomInviteUrl ?? DblApi.GetDefaultInviteUrl(Id);

        [JsonProperty("certified")]
		public bool IsCertified { get; }

        [JsonProperty("lib")]
		public string LibraryUsed { get; }

        [JsonProperty("longDesc")]
		public string LongDescription { get; }

        [JsonProperty("prefix")]
		public string PrefixUsed { get; }

		[JsonProperty("owners")]
		public List<ulong> OwnerIds { get; }

		IReadOnlyList<ulong> IDblBot.OwnerIds => OwnerIds;

        [JsonProperty("points")]
		public int Points { get; }

        [JsonProperty("monthlypoints")]
		public int MonthlyPoints { get; }

        [JsonProperty("shortDesc")]
		public string ShortDescription { get; }

        [JsonProperty("tags")]
		public List<string> Tags { get; }

		IReadOnlyList<string> IDblBot.Tags => Tags;

		public string SupportInviteUrl => $"https://discord.gg/{SupportInviteCode}";

		[JsonProperty("vanity")]
        internal string VanityName { get; }

        public string VanityUrl => string.IsNullOrWhiteSpace(VanityName) ? DblApi.GetVanityUrl(VanityName) : DblApi.GetDefaultVanityUrl(Id);

        [JsonProperty("website")]
		public string WebsiteUrl { get; }

		public IReadOnlyList<ulong> FeaturedGuildIds => throw new NotImplementedException();

		public ulong PrimaryOwnerId => OwnerIds.First();

		public string SupportGuildUrl => throw new NotImplementedException();

		public ulong? DonateBotGuildId => throw new NotImplementedException();

		public async Task<IDblBotStats> GetStatsAsync()
			=> await Client.GetBotStatsAsync(Id);
    }
}
