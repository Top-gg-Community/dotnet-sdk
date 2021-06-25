using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class Bot : Entity, IDblBot
    {
        internal DiscordBotListApi api;

        [JsonProperty("lib")]
        internal string library;

        [JsonProperty("prefix")]
        internal string prefix;

        [JsonProperty("shortDesc")]
        internal string shortDescription;

        [JsonProperty("longDesc")]
        internal string longDescription;

        [JsonProperty("tags")]
        internal List<string> tags;

        [JsonProperty("website")]
        internal string websiteUrl;

        [JsonProperty("support")]
        internal string supportUrl;

        [JsonProperty("github")]
        internal string githubUrl;

        [JsonProperty("owners")]
        internal List<ulong> owners;

        [JsonProperty("invite")]
        internal string customInvite;

        [JsonProperty("date")]
        internal DateTime approvedAt;

        [JsonProperty("certified")]
        internal bool certified;

        [JsonProperty("vanity")]
        internal string vanity;

        [JsonProperty("points")]
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
        public string VanityUrl => "https://top.gg/bots/" + vanity;

        public string WebsiteUrl => websiteUrl;

        public async Task<IDblBotStats> GetStatsAsync()
            => await api.GetBotStatsAsync(Id);
    }
}