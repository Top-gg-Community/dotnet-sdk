using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Internal.Queries;
using DiscordBotsList.Api.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordBotsList.Api
{
    public enum SortBotsBy
    {
        MonthlyPoints,
        Id,
        Date,
    }

    /// <summary>
    ///     Authenticated client for the legacy Top.gg <c>v0</c> API
    ///     (<c>https://top.gg/api</c>). v0 remains functional for existing projects but
    ///     does not receive new features. New integrations should use
    ///     <see cref="TopggV1Client" /> with a v1 (Bearer) token instead.
    /// </summary>
    public class AuthDiscordBotListApi : DiscordBotListApi
    {
        private readonly ulong _selfId;

        public AuthDiscordBotListApi(ulong selfId, string token)
        {
            _selfId = selfId;
            // The legacy v0 API expects the token directly in the Authorization header,
            // without the Bearer prefix. Use TopggV1Client for the current v1 API, which
            // requires `Authorization: Bearer <token>`.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
        }

        /// <summary>
        ///     Fetches bots from Top.gg
        /// </summary>
        /// <param name="count">amount of bots to retrieve (max: 500)</param>
        /// <param name="offset">amount of bots to skip</param>
        /// <param name="sortBy">sorts results based on their monthly vote count, id, or their submission date</param>
        /// <returns>List of Bot Objects</returns>
        public async Task<ISearchResult<IDblBot>> GetBotsAsync(int count = 50, int offset = 0, SortBotsBy sortBy = SortBotsBy.MonthlyPoints)
        {
            var sortByString = sortBy.ToString();
            sortByString = char.ToLowerInvariant(sortByString[0]) + sortByString[1..];

            var result = await GetAsync<BotListQuery>($"bots?limit={count}&offset={offset}&sort={sortByString}");

            foreach (var bot in result.Items) (bot as Bot).api = this;
            return result;
        }

        /// <summary>
        ///     Template
        ///     of GetBotAsync for internal usage.
        /// </summary>
        /// <typeparam name="T">Type of Bot</typeparam>
        /// <param name="id">Discord id</param>
        /// <returns>Bot object of type T</returns>
        internal async Task<T> GetBotAsync<T>(ulong id) where T : Bot
        {
            var t = await GetAsync<T>($"bots/{id}");
            if (t == null) return null;
            t.api = this;
            return t;
        }

        /// <summary>
        ///     Get specific bot by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>Bot Object</returns>
        public new async Task<IDblBot> GetBotAsync(ulong id)
        {
            return await GetBotAsync<Bot>(id);
        }

        /// <summary>
        ///     Get bot stats
        /// </summary>
        /// <param name="id">Discord id, no longer needed</param>
        /// <returns>IBotStats object related to the bot</returns>
        public new async Task<IDblBotStats> GetBotStatsAsync(ulong id = 0)
        {
            return await GetAsync<BotStatsObject>($"bots/{_selfId}/stats");
        }

        /// <summary>
        ///     Get specific user by Discord id
        /// </summary>
        /// <param name="id">Discord id</param>
        /// <returns>User Object</returns>
        public new async Task<IDblUser> GetUserAsync(ulong id)
        {
            return await GetAsync<User>($"users/{id}");
        }

        /// <summary>
        ///     Gets your own bot with as an ISelfBot
        /// </summary>
        /// <returns>your own bot with as an ISelfBot</returns>
        public async Task<IDblSelfBot> GetMeAsync()
        {
            var bot = await GetBotAsync<SelfBot>(_selfId);
            bot.api = this;
            return bot;
        }

        /// <summary>
        ///     Fetches unique voters that have voted for your project
        /// </summary>
        /// <param name="page">The page number, defaults to 1</param>
        /// <returns>A list of voters</returns>
        public async Task<List<IDblEntity>> GetVotersAsync(int page = 1)
        {
            return (await GetAsync<List<Entity>>($"bots/{_selfId}/votes?page={Math.Max(page, 1)}")).Cast<IDblEntity>().ToList();
        }

        /// <summary>
        ///     Update your stats unsharded
        /// </summary>
        /// <param name="guildCount">count of guilds</param>
        public async Task UpdateStats(int guildCount)
        {
            await UpdateStatsAsync(new GuildCountObject(guildCount));
        }

        /// <summary>
        ///     Update your stats sharded
        /// </summary>
        /// <param name="shardId">Begin shard id</param>
        /// <param name="shardCount">Total shards</param>
        /// <param name="shards">Guild count per shards</param>
        public async Task UpdateStats(int shardId, int shardCount, params int[] shards)
        {
            await UpdateStatsAsync(new ShardedGuildCountObject
            {
                ShardId = shardId,
                ShardCount = shardCount,
                Shards = shards
            });
        }

        /// <summary>
        ///     Update your stats sharded
        /// </summary>
        /// <param name="guildCount">count of guilds</param>
        /// <param name="shardCount">Total shards</param>
        public async Task UpdateStats(int guildCount, int shardCount)
        {
            await UpdateStatsAsync(new ShardedGuildCountObject
            {
                ShardCount = shardCount,
                GuildCount = guildCount
            });
        }

        /// <summary>
        ///     returns true if the user has voted for your project in the past 12 hours
        /// </summary>
        /// <param name="userId">the user ID</param>
        /// <returns>True or False</returns>
        public async Task<bool> HasVoted(ulong userId)
        {
            return await HasVotedAsync(userId);
        }

        protected async Task<List<T>> GetVotersAsync<T>()
        {
            var query = $"bots/{_selfId}/votes";
            return await GetAuthorizedAsync<List<T>>(Utils.CreateQuery(query));
        }

        protected async Task UpdateStatsAsync(object statsObject)
        {
            var json = JsonSerializer.Serialize(statsObject);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient
                .PostAsync($"{baseEndpoint}/bots/{_selfId}/stats", httpContent);
        }

        protected Task<T> GetAuthorizedAsync<T>(string url)
        {
            return GetAsync<T>(url);
        }

        protected async Task<bool> HasVotedAsync(ulong userId)
        {
            var url = $"bots/{_selfId}/check?userId={userId}";
            return (await GetAsync<HasVotedObject>(url)).HasVoted.GetValueOrDefault(0) == 1;
        }
    }
}