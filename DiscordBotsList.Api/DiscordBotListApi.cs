﻿using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Internal.Queries;
using Newtonsoft.Json;
using Miki.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
	public class DiscordBotListApi
	{
		protected RestClient restClient = new RestClient("https://discordbots.org/api/");

		public DiscordBotListApi()
		{ }

		/// <summary>
		/// Gets bots from botlist 
		/// </summary>
		/// <param name="count">amount of bots to appear per page (max: 500)</param>
		/// <param name="page">current page to query</param>
		/// <returns>List of Bot Objects</returns>
		public async Task<ISearchResult<IBot>> GetBotsAsync(int count = 50, int page = 0)
			=> await GetAsync<BotListQuery>("bots");

		/// <summary>
		/// Get specific bot by Discord id
		/// </summary>
		/// <param name="id">Discord id</param>
		/// <returns>Bot Object</returns>
		public async Task<IBot> GetBotAsync(ulong id)
			=> await GetBotAsync<Bot>(id);

		/// <summary>
		/// Get bot stats
		/// </summary>
		/// <param name="id">Discord id</param>
		/// <returns>IBotStats object related to the bot</returns>
		public async Task<IBotStats> GetBotStatsAsync(ulong id)
			=> await GetAsync<BotStatsObject>($"bots/{id}/stats");

		/// <summary>
		/// Get specific user by Discord id
		/// </summary>
		/// <param name="id">Discord id</param>
		/// <returns>User Object</returns>
		public async Task<IUser> GetUserAsync(ulong id)
			=> await GetAsync<User>($"users/{id}");

		/// <summary>
		/// Template version of GetBotAsync for internal usage.
		/// </summary>
		/// <typeparam name="T">Type of Bot</typeparam>
		/// <param name="id">Discord id</param>
		/// <returns>Bot object of type T</returns>
		protected async Task<T> GetBotAsync<T>(ulong id) where T : Bot
		{
			T t = await GetAsync<T>($"bots/{id}");
			t.api = this;
			return t;
		}

		/// <summary>
		/// Gets and parses objects
		/// </summary>
		/// <typeparam name="T">Type to parse to</typeparam>
		/// <param name="url">Url to get from</param>
		/// <returns>Object of type T</returns>
		protected async Task<T> GetAsync<T>(string url)
		{
			RestResponse<T> t = await restClient.GetAsync<T>(url);
			if (t.Success)
			{
				return t.Data;
			}
			return default(T);
		}
	}
}
