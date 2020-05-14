using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Internal.Queries;
using System.Threading.Tasks;
using DiscordBotsList.Api.Objects;
using System.Net.Http;
using Newtonsoft.Json;

namespace DiscordBotsList.Api
{
	/// <summary>
	/// Represents the base of a Discord Bot List API client.
	/// </summary>
	public class BaseDblClient
	{
		protected HttpClient _httpClient;
        protected const string baseEndpoint = "https://top.gg/api/";

		public BaseDblClient()
		{
            _httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a list of all bots on a specified search.
		/// </summary>
		/// <param name="count">Represents the amount of bots that can appear per page (Max: 500).</param>
		/// <param name="page">Represents the page at which this search result starts.</param>
		/// <returns>A list of bots from the specified search.</returns>
		public async Task<ISearchResult<IDblBot>> GetBotsAsync(int count = 50, int page = 0)
		{
			count = count > 500 ? 500 : count;

			var result = await GetAsync<BotListQuery>("bots");

			foreach(IDblBot bot in result.Items)
			{
				(bot as Bot).api = this;
			}

			return result;
		}

		/// <summary>
		/// Returns the bot specified by its ID. If no results were found, this returns null.
		/// </summary>
		/// <param name="id">Represents the ID of the bot you wish to retrieve.</param>
		/// <returns>A generic object representing a bot.</returns>
		public async Task<IDblBot> GetBotAsync(ulong id)
			=> await GetBotAsync<Bot>(id);

		/// <summary>
		/// Returns the stats of a bot specified by its ID. If no results were found, this returns null.
		/// </summary>
		/// <param name="id">Represents the ID of the bot you wish to retrieve stats for.</param>
		/// <returns>An object that represents the stats for a specified bot.</returns>
		public async Task<IDblBotStats> GetBotStatsAsync(ulong id)
			=> await GetAsync<BotStatsObject>($"bots/{id}/stats");

		/// <summary>
		/// Returns the user specified by its ID. If no results were found, this returns null.
		/// </summary>
		/// <param name="id">Represents the ID of the user you wish to retrieve.</param>
		/// <returns>A generic object representing a user.</returns>
		public async Task<IDblUser> GetUserAsync(ulong id)
			=> await GetAsync<User>($"users/{id}");

		/// <summary>
		/// Returns the instance of a bot by its ID.
		/// </summary>
		/// <typeparam name="T">Represents the object type that this bot will be returned as.</typeparam>
		/// <param name="id">Represents the ID of the bot that you wish to retrieve.</param>
		/// <returns>An object representing a bot.</returns>
		internal async Task<T> GetBotAsync<T>(ulong id)
			where T : Bot
		{
			T t = await GetAsync<T>($"bots/{id}");
			t.api = this;
			return t;
		}

		/// <summary>
		/// Returns the object of a custom request with a provided URL.
		/// </summary>
		/// <typeparam name="T">Represents the type that this request will be cast to.</typeparam>
		/// <param name="url">Represents the partial URL that will be used for this call.</param>
		/// <returns>An object of type T from the specified URL.</returns>
		protected async Task<T> GetAsync<T>(string url)
		{
			HttpResponseMessage t = await _httpClient.GetAsync(baseEndpoint + url);
            if (t.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<T>(await t.Content.ReadAsStringAsync());
			return default;
		}

	    /// <summary>
	    /// Returns a bool that defines if it is currently the weekend. If this returns true, the voting multiplier is doubled.
	    /// </summary>
	    /// <returns>A boolean value that defines if it is currently the weekend.</returns>
	    public async Task<bool> IsWeekendAsync()
			=> (await GetAsync<WeekendObject>("weekend")).Weekend;
	}
}
