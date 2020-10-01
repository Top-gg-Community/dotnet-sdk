using System;
using System.Threading.Tasks;
using System.Net.Http;
using DiscordBotList.Internal;
using DiscordBotList.Models;
using Newtonsoft.Json;

namespace DiscordBotList
{
	/// <summary>
	/// Represents the base of a Discord Bot List API client.
	/// </summary>
	public class BaseDblClient
	{
		/// <summary>
		/// Initializes a new <see cref="BaseDblClient"/>.
		/// </summary>
        public BaseDblClient()
		{
            HttpClient = new HttpClient();
		}

		~BaseDblClient()
        {
			HttpClient.Dispose();
        }

        protected HttpClient HttpClient { get; }

		/// <summary>
		/// Asynchronously retrieves a list of all bots on a specified search.
		/// </summary>
		/// <param name="limit">The total amount of bots that can appear for this request (Max: 500).</param>
		/// <param name="offset">The offset at which this request starts.</param>
		/// <returns>An <see cref="ISearchResult{T}"/> response containing the successfully received values.</returns>
		public async Task<ISearchResult<IDblBot>> GetBotsAsync(int limit = BotSearchParams.DefaultLimit, int offset = BotSearchParams.DefaultOffset)
        {
            var searchParams = new BotSearchParams
            {
				Limit = limit,
				Offset = offset
            };

			var result = await GetAsync<BotSearchResult>(DblApi.GetSearchEndpoint(searchParams));

			foreach(DblBot bot in result.Values)
                bot.Client = this;

            return result;
		}

        /// <summary>
        /// Asynchronously retrieves a list of all bots on a specified search.
        /// </summary>
        /// <param name="searchParams">The search parameters to use for this request.</param>
        /// <returns>A list of bots from the specified search.</returns>
        public async Task<ISearchResult<IDblBot>> GetBotsAsync(BotSearchParams searchParams)
        {
            searchParams = searchParams ?? new BotSearchParams
            {
                Limit = BotSearchParams.DefaultLimit,
                Offset = BotSearchParams.DefaultOffset
            };

            var result = await GetAsync<BotSearchResult>(DblApi.GetSearchEndpoint(searchParams));

            foreach (DblBot bot in result.Values)
                bot.Client = this;

            return result;
        }

		/// <summary>
		/// Asynchronously retrieves the bot specified by its unique identifier (nullable).
		/// </summary>
		/// <param name="id">The unique identifier of the bot you wish to retrieve.</param>
		/// <returns>A generic <see cref="IDblBot"/> object.</returns>
		public async Task<IDblBot> GetBotAsync(ulong id)
			=> await GetBotAsync<DblBot>(id);

		/// <summary>
		/// Asynchronously retrieves the stats of a bot specified by its unique identifier (nullable).
		/// </summary>
		/// <param name="id">The unique identifier of the bot you wish to retrieve stats for.</param>
		/// <returns>A generic <see cref="IDblBotStats"/> object.</returns>
		public async Task<IDblBotStats> GetBotStatsAsync(ulong id)
			=> await GetAsync<DblBotStats>(DblApi.GetStatsEndpoint(id));

		/// <summary>
		/// Asynchronously retrieves the user specified by their unique identifier (nullable).
		/// </summary>
		/// <param name="id">The unique identifier of the user you wish to retrieve.</param>
		/// <returns>A generic <see cref="IDblUser"/> object.</returns>
		public async Task<IDblUser> GetUserAsync(ulong id)
			=> await GetAsync<DblUser>(DblApi.GetUserEndpoint(id));

        /// <summary>
        /// Asynchronously retrieves a value that defines if it is currently the weekend. If this returns true, the voting multiplier is doubled.
        /// </summary>
        /// <returns>A boolean value that specifies if it is currently the weekend.</returns>
        public async Task<bool> IsWeekendAsync()
            => (await GetAsync<WeekendResult>(DblApi.WeekendEndpoint)).IsWeekend;

		/// <summary>
		/// Asynchronously retrieves a bot specified by their unique identifier (nullable).
		/// </summary>
		/// <typeparam name="T">Represents the object type that this bot will be returned as.</typeparam>
		/// <param name="id">The unique identifier of the bot that you wish to retrieve.</param>
		/// <returns>A <see cref="T"/> instance from the specified unique identifier.</returns>
		internal async Task<T> GetBotAsync<T>(ulong id)
			where T : DblBot
		{
			var t = await GetAsync<T>(DblApi.GetBotEndpoint(id));
			t.Client = this;
			return t;
		}

		/// <summary>
		/// Returns the object of a custom request with a provided endpoint.
		/// </summary>
		/// <typeparam name="T">The type that this request will be cast to.</typeparam>
		/// <param name="endpoint">The endpoint that will be appended to the base URL for this call.</param>
		/// <returns>An object of type T from the specified URL.</returns>
		protected async Task<T> GetAsync<T>(string endpoint)
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{DblApi.BaseUrl}{endpoint}");
			ApiResult<T> result;
			try
			{
				result = response.IsSuccessStatusCode
					? ApiResult<T>.FromSuccess(JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()))
					: ApiResult<T>.FromHttpError(response.StatusCode);
			}
			catch (Exception ex)
			{
				result = ApiResult<T>.FromError(ex);
			}

			if (!result.IsSuccess)
				throw new Exception(result.ErrorReason);

			if (result.Value == null)
				return default;

			return result.Value;
        }
    }
}
