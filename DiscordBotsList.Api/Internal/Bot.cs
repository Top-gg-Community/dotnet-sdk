using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotsList.Api.Internal
{
    internal class Bot : Entity, IBot
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
		internal int score;

		public DateTime ApprovedAt => approvedAt;

		public string GithubUrl => githubUrl;

		public string InviteUrl => customInvite ?? $"https://discordapp.com/oauth2/authorize?&client_id={ Id }&scope=bot";

		public bool IsCertified => certified;

		public string LibraryUsed => library;

		public string LongDescription => longDescription;

		public string PrefixUsed => prefix;

		public List<ulong> OwnerIds => owners.ToList();

		public int Score => score;

		public string ShortDescription => shortDescription;

		public List<string> Tags => tags.ToList();

		public string SupportUrl => "https://discord.gg/" + supportUrl;

		public string VanityTag => vanity;
		public string VanityUrl => "https://discordbots.org/bots/" + vanity;

		public string WebsiteUrl => websiteUrl;

		public async Task<IBotStats> GetStatsAsync()
			=> await api.GetBotStatsAsync(Id);
	}
}
