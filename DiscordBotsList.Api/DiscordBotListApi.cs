using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Internal.Queries;
using DiscordBotsList.Api.Objects;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    public class DiscordBotListApi
    {
        protected HttpClient _httpClient;
        protected const string baseEndpoint = "https://top.gg/api/";

        public DiscordBotListApi()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets bots from botlist
        /// </summary>
        /// <param name="count">amount of bots to appear per page (max: 500)</param>
        /// <param name="page">current page to query</param>
        /// <returns>List of Bot Objects</returns>
        public async Task<ISearchResult<IDblBot>> GetBotsAsync(int count = 50, int page = 0)
        {
            var result = await GetAsync<BotListQuery>("bots");

            foreach (var bot in result.Items)
                (bot as Bot).api = this;

            return result;
        }

        /// <summary>
        /// Get specific bot by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>Bot Object</returns>
        public async Task<IDblBot> GetBotAsync(ulong id)
            => await GetBotAsync<Bot>(id);

        /// <summary>
        /// Get bot stats
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>IBotStats object related to the bot</returns>
        public async Task<IDblBotStats> GetBotStatsAsync(ulong id)
            => await GetAsync<BotStatsObject>($"bots/{id}/stats");

        /// <summary>
        /// Get specific user by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>User Object</returns>
        public async Task<IDblUser> GetUserAsync(ulong id)
            => await GetAsync<User>($"users/{id}");

        /// <summary>
        /// Template version of GetBotAsync for internal usage.
        /// </summary>
        /// <typeparam name="T">Type of Bot</typeparam>
        /// <param name="id">Discord id</param>
        /// <returns>Bot object of type T</returns>
        internal async Task<T> GetBotAsync<T>(ulong id) where T : Bot
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
            HttpResponseMessage t = await _httpClient.GetAsync(baseEndpoint + url);
            ApiResult<T> result;
            try
            {
                result = t.IsSuccessStatusCode ? ApiResult<T>.FromSuccess(JsonConvert.DeserializeObject<T>(await t.Content.ReadAsStringAsync())) : ApiResult<T>.FromHttpError(t.StatusCode);
            }
            catch (Exception ex)
            {
                result = ApiResult<T>.FromError(ex);
            }
            return result.Value;
        }

        /// <summary>
        /// returns true if voting multiplier = x2
        /// </summary>
        /// <returns>True or False</returns>
        public async Task<bool> IsWeekendAsync()
            => (await GetAsync<WeekendObject>("weekend")).Weekend;
    }
}