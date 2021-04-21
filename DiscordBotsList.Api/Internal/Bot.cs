using DiscordBotsList.Api.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Internal
{
    internal class Bot : Entity, IDblBot
    {
        internal DiscordBotListApi api;

        [JsonPropertyName("lib")]
        internal string library;

        [JsonPropertyName("prefix")]
        internal string prefix;

        [JsonPropertyName("shortDesc")]
        internal string shortDescription;

        [JsonPropertyName("longDesc")]
        internal string longDescription;

        [JsonPropertyName("tags")]
        internal List<string> tags;

        [JsonPropertyName("website")]
        internal string websiteUrl;

        [JsonPropertyName("support")]
        internal string supportUrl;

        [JsonPropertyName("github")]
        internal string githubUrl;

        [JsonPropertyName("owners")]
        internal List<ulong> owners;

        [JsonPropertyName("invite")]
        internal string customInvite;

        [JsonPropertyName("date")]
        internal DateTime approvedAt;

        [JsonPropertyName("certified")]
        internal bool certified;

        [JsonPropertyName("vanity")]
        internal string vanity;

        [JsonPropertyName("points")]
        internal int points;

        public DateTime ApprovedAt => approvedAt;

        public string GithubUrl => githubUrl;

        public string InviteUrl => customInvite ?? $"https://discord.com/oauth2/authorize?&client_id={ Id }&scope=bot";

        public bool IsCertified => certified;

        public string LibraryUsed => library;

        public string LongDescription => longDescription;

        public string PrefixUsed => prefix;

        public List<ulong> OwnerIds => owners.ToList();

        public int Points => points;

        public string ShortDescription => shortDescription;

        public List<string> Tags => tags.ToList();

        public string SupportUrl => "https://discord.gg/" + supportUrl;

        public string VanityTag => vanity;
        public string VanityUrl => "https://top.gg/bot/" + vanity;

        public string WebsiteUrl => websiteUrl;

        public async Task<IDblBotStats> GetStatsAsync()
            => await api.GetBotStatsAsync(Id);
    }
}