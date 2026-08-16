using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Objects;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Base client for the legacy Top.gg <c>v0</c> API
    ///     (<c>https://top.gg/api</c>). Use <see cref="TopggV1Client" /> for the current
    ///     v1 API.
    /// </summary>
    public class DiscordBotListApi
    {
        protected const string baseEndpoint = "https://top.gg/api/";
        private readonly JsonSerializerOptions _serializerOptions;
        protected readonly HttpClient _httpClient;

        public DiscordBotListApi()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new ULongToStringConverter());
        }

        /// <summary>
        ///     Gets bots from botlist
        /// </summary>
        /// <param name="count">amount of bots to appear per page (max: 500)</param>
        /// <param name="page">current page to query</param>
        /// <returns>List of Bot Objects</returns>
        [Obsolete("This method requires a token to work. Please use the AuthenticatedBotListApi class instead.", true)]
        public Task<ISearchResult<IDblBot>> GetBotsAsync(int count = 50, int page = 0)
        {
            return null;
        }

        /// <summary>
        ///     Get specific bot by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>Bot Object</returns>
        [Obsolete("This method requires a token to work. Please use the AuthenticatedBotListApi class instead.", true)]
        public Task<IDblBot> GetBotAsync(ulong id)
        {
            return null;
        }

        /// <summary>
        ///     Get bot stats
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>IBotStats object related to the bot</returns>
        [Obsolete("This method requires a token to work. Please use the AuthenticatedBotListApi class instead.", true)]
        public Task<IDblBotStats> GetBotStatsAsync(ulong id)
        {
            return null;
        }

        /// <summary>
        ///     Get specific user by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>User Object</returns>
        [Obsolete("This method requires a token to work. Please use the AuthenticatedBotListApi class instead.", true)]
        public Task<IDblUser> GetUserAsync(ulong id)
        {
            return null;
        }

        /// <summary>
        ///     Gets and parses objects
        /// </summary>
        /// <typeparam name="T">Type to parse to</typeparam>
        /// <param name="url">Url to get from</param>
        /// <returns>Object of type T, or default when the request is not successful</returns>
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(baseEndpoint + url);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<T>(_serializerOptions)
                : default;
        }

        /// <summary>
        ///     returns true if voting multiplier = x2
        /// </summary>
        /// <returns>True or False</returns>
        public async Task<bool> IsWeekendAsync()
        {
            return (await GetAsync<WeekendObject>("weekend")).Weekend;
        }
    }
}