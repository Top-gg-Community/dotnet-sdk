using DiscordBotsList.Api.Internal;
using Miki.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    public class AuthDiscordBotListApi : DiscordBotListApi
    {
		private ulong selfId;
		private string token;

		public AuthDiscordBotListApi(ulong selfId, string token)
		{
			this.selfId = selfId;
			this.token = token;
		}

		/// <summary>
		/// Gets your own bot with as an ISelfBot
		/// </summary>
		/// <returns>your own bot with as an ISelfBot</returns>
		public async Task<ISelfBot> GetMeAsync()
		{
			SelfBot bot = await GetBotAsync<SelfBot>(selfId);
			bot.api = this;
			return bot;
		}

		/// <summary>
		/// Gets all voters that have voted on your bot
		/// </summary>
		/// <param name="days">Amount of days to filter</param>
		/// <returns>A list of voters</returns>
		public async Task<List<IEntity>> GetVotersAsync(int? days)
			=> (await GetVotersAsync<Entity>(days)).Cast<IEntity>().ToList();

		/// <summary>
		/// Gets all voter ids that have voted on your bot
		/// </summary>
		/// <param name="days">Amount of days to filter</param>
		/// <returns>A list of snowflakes</returns>
		public async Task<List<ulong>> GetVoterIdsAsync(int? days)
			=> await GetVotersAsync<ulong>(days);

		/// <summary>
		/// Update your stats unsharded
		/// </summary>
		/// <param name="guildCount">count of guilds</param>
		public async Task UpdateStats(int guildCount)
			=> await UpdateStatsAsync(new GuildCountObject(guildCount));
		/// <summary>
		/// Update your stats sharded
		/// </summary>
		/// <param name="shardId">Begin shard id</param>
		/// <param name="shardCount">Total shards</param>
		/// <param name="shards">Guild count per shards</param>
		public async Task UpdateStats(int shardId, int shardCount, params int[] shards)
			=> await UpdateStatsAsync(new ShardedGuildCountObject() { ShardId = shardId, ShardCount = shardCount, Shards = shards });

		protected async Task<List<T>> GetVotersAsync<T>(int? days)
		{
			string query = $"bots/{selfId}/votes";
			List<string> args = new List<string>();

			if (typeof(T).IsPrimitive)
				args.Add("onlyids=true");

			if (days != null)
				args.Add($"days={days}");

			return await GetAuthorizedAsync<List<T>>(Utils.CreateQuery(query, args.ToArray()));
		}

		protected async Task UpdateStatsAsync(object statsObject)
		{
			string json = JsonConvert.SerializeObject(statsObject);

			await restClient
				.SetAuthorization(token)
				.PostAsync($"bots/{selfId}/stats", json);
		}

		protected async Task<T> GetAuthorizedAsync<T>(string url)
		{
			RestResponse<T> t = await restClient
				.SetAuthorization(token)
				.GetAsync<T>(url);
			if (t.Success)
			{
				return t.Data;
			}
			return default(T);
		}
	}
}
