using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Objects;
using Newtonsoft.Json;

namespace DiscordBotsList.Api
{
    public class AuthDiscordBotListApi : DiscordBotListApi
    {
        private readonly ulong _selfId;
        private readonly string _token;

        public AuthDiscordBotListApi(ulong selfId, string token)
        {
            _selfId = selfId;
            _token = token;
        }

        /// <summary>
        /// Gets your own bot with as an ISelfBot
        /// </summary>
        /// <returns>your own bot with as an ISelfBot</returns>
        public async Task<IDblSelfBot> GetMeAsync()
        {
            var bot = await GetBotAsync<SelfBot>(_selfId);
            bot.api = this;
            return bot;
        }

        /// <summary>
        /// Gets all voters that have voted on your bot
        /// Max 1000, If you have more, you MUST use WEBHOOKS instead.
        /// </summary>
        /// <returns>A list of voters</returns>
        public async Task<List<IDblEntity>> GetVotersAsync()
        {
            return (await GetVotersAsync<Entity>()).Cast<IDblEntity>().ToList();
        }

        /// <summary>
        /// returns true if user have voted for the past 12 hours
        /// </summary>
        /// <param name="userId">Amount of days to filter</param>
        /// <returns>True or False</returns>
        public async Task<bool> HasVoted(ulong userId)
        {
            return await HasVotedAsync(userId);
        }

        /// <summary>
        /// returns true if voting multiplier = x2
        /// </summary>
        /// <returns>True or False</returns>
        public async Task<bool> IsWeekend()
        {
            return await IsWeekendAsync();
        }

        /// <summary>
        /// Update your stats unsharded
        /// </summary>
        /// <param name="guildCount">count of guilds</param>
        public async Task UpdateStats(int guildCount)
        {
            await UpdateStatsAsync(new GuildCountObject(guildCount));
        }

        /// <summary>
        /// Update your stats sharded
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


        protected async Task<List<T>> GetVotersAsync<T>()
        {
            var points = GetMeAsync().Result.Points;
            if (points > 1000)
                throw new System.InvalidOperationException("You have more than 1000 points. You must use webhooks.");
            var query = $"bots/{_selfId}/votes";
            return await GetAuthorizedAsync<List<T>>(Utils.CreateQuery(query));
        }


        protected async Task<bool> HasVotedAsync(ulong userId)
        {
            var url = "https://discordbots.org/api/bots/" + $"{_selfId}/check?userId={userId}";
            var response = await RestClient.SetAuthorization(_token).GetAsync(url);
            return response.Body.Contains('1');
        }

        protected async Task<bool> IsWeekendAsync()
        {
            var url = "https://discordbots.org/api/weekend";
            var response = await RestClient.SetAuthorization(_token).GetAsync(url);
            return response.Body.Contains("true");
        }

        protected async Task UpdateStatsAsync(object statsObject)
        {
            var json = JsonConvert.SerializeObject(statsObject);

            await RestClient
                .SetAuthorization(_token)
                .PostAsync($"bots/{_selfId}/stats", json);
        }

        protected async Task<T> GetAuthorizedAsync<T>(string url)
        {
            var t = await RestClient
                .SetAuthorization(_token)
                .GetAsync<T>(url);
            if (t.Success) return t.Data;
            return default(T);
        }
    }
}