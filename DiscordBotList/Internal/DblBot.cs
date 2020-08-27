using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotList.Models;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblBot"/>
    internal class DblBot : DblEntity, IDblBot
	{
        /// <inheritdoc />
        [JsonProperty("date")]
        public DateTime ApprovedAt { get; internal set; }

        [JsonProperty("guilds")]
        public List<ulong> FeaturedGuildIds { get; internal set; }

        IReadOnlyList<ulong> IDblBot.FeaturedGuildIds => FeaturedGuildIds;

        /// <inheritdoc />
        public ulong PrimaryOwnerId => OwnerIds.First();

        [JsonProperty("owners")]
        public List<ulong> OwnerIds { get; internal set; }

        IReadOnlyList<ulong> IDblBot.OwnerIds => OwnerIds;

        /// <inheritdoc />
        [JsonProperty("lib")]
        public string LibraryUsed { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("prefix")]
        public string PrefixUsed { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("shortDesc")]
        public string ShortDescription { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("longDesc")]
        public string LongDescription { get; internal set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; internal set; }

        IReadOnlyList<string> IDblBot.Tags => Tags;

        [JsonProperty("vanity")]
        internal string VanityName { get; set; }

        /// <inheritdoc />
        public string VanityUrl => DblApi.GetVanityUrl(VanityName, Id);

        /// <inheritdoc />
        [JsonProperty("website")]
        public string WebsiteUrl { get; internal set; }

        [JsonProperty("support")]
        internal string SupportInviteCode { get; set; }

        /// <inheritdoc />
        public string SupportInviteUrl => DblApi.GetInviteUrl(SupportInviteCode);

        /// <inheritdoc />
        [JsonProperty("github")]
        public string GitHubUrl { get; internal set; }

        [JsonProperty("invite")]
        internal string CustomInviteUrl { get; set; }

        /// <inheritdoc />
		public string InviteUrl => CustomInviteUrl ?? DblApi.GetDefaultInviteUrl(Id);

        /// <inheritdoc />
        [JsonProperty("certified")]
		public bool IsCertified { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("points")]
		public int Points { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("monthlypoints")]
		public int MonthlyPoints { get; internal set; }

        [JsonProperty("donatebot")]
        internal ulong DonateBotId { get; set; }

        /// <inheritdoc />
        public ulong? DonateBotGuildId => DonateBotId == 0 ? (ulong?)null : DonateBotId;

        internal BaseDblClient Client { get; set; }

        public async Task<IDblBotStats> GetStatsAsync()
			=> await Client.GetBotStatsAsync(Id);
    }
}
