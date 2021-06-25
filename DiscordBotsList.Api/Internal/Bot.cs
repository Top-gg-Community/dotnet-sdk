using DiscordBotsList.Api.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Internal
{
    public class Bot : Entity, IDblBot
    {
        internal DiscordBotListApi api;

        [JsonPropertyName("lib")]
        [JsonInclude]
        internal string library;

        [JsonPropertyName("prefix")]
        [JsonInclude]
        internal string prefix;

        [JsonPropertyName("shortDesc")]
        [JsonInclude]
        internal string shortDescription;

        [JsonPropertyName("longDesc")]
        [JsonInclude]
        internal string longDescription;

        [JsonPropertyName("tags")]
        [JsonInclude]
        internal List<string> tags;

        [JsonPropertyName("website")]
        [JsonInclude]
        internal string websiteUrl;

        [JsonPropertyName("support")]
        [JsonInclude]
        internal string supportUrl;

        [JsonPropertyName("github")]
        [JsonInclude]
        internal string githubUrl;

        [JsonPropertyName("owners")]
        [JsonInclude]
        internal List<ulong> owners;

        [JsonPropertyName("invite")]
        [JsonInclude]
        internal string customInvite;

        [JsonPropertyName("date")]
        [JsonInclude]
        internal DateTime approvedAt;

        [JsonPropertyName("certified")]
        [JsonInclude]
        internal bool certified;

        [JsonPropertyName("vanity")]
        [JsonInclude]
        internal string vanity;

        [JsonPropertyName("points")]
        [JsonInclude]
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