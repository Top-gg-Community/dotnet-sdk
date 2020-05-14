using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DiscordBotsList.Api.Internal;
using DiscordBotsList.Api.Objects;
using Newtonsoft.Json;

namespace DiscordBotsList.Api
{
    /// <summary>
    /// Represents an authenticated Discord Bot List API client.
    /// </summary>
    public class DblClient : BaseDblClient
    {
        private readonly ulong _id;

        /// <summary>
        /// Initializes a new <see cref="DblClient"/> using the specified ID and token.
        /// </summary>
        /// <param name="id">Represents the ID of the bot that this <see cref="DblClient"/> will be bound to.</param>
        /// <param name="token">Represents the token used to access the API.</param>
        public DblClient(ulong id, string token)
			: base()
        {
            _id = id;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Returns an instance of the bot that this <see cref="DblClient"/> is bound to.
        /// </summary>
        /// <returns>The object of a bot that the <see cref="DblClient"/> is bound to.</returns>
        public async Task<IDblSelfBot> GetSelfAsync()
        {
            var bot = await GetBotAsync<SelfBot>(_id);
            bot.api = this;
            return bot;
        }

        /// <summary>
        /// Returns a list of all users that have voted for your bot for up to 1,000 entries. If you have more than 1,000, you are required to use webhooks instead.
        /// </summary>
        /// <returns>A list of all users that have voted.</returns>
        public async Task<List<IDblEntity>> GetVotersAsync()
        {
            return (await GetVotersAsync<Entity>()).Cast<IDblEntity>().ToList();
        }

        /// <summary>
        /// Sends a request to update your stats by guild count.
        /// </summary>
        /// <param name="guildCount">Represents the current guild count for the bot.</param>
        public async Task UpdateStatsAsync(int guildCount)
        {
            await UpdateStatsInternalAsync(new GuildCountObject(guildCount));
        }

        /// <summary>
        /// Sends a request to update your stats by specific shard values.
        /// </summary>
        /// <param name="startIndex">Represents the starting index at which to update your shards from.</param>
        /// <param name="shardCount">Represents the current shard count that the bot uses.</param>
        /// <param name="shards">Represents an array of guild counts for each shard, starting from the specified index.</param>
        public async Task UpdateStatsAsync(int startIndex, int shardCount, params int[] shards)
        {
            await UpdateStatsInternalAsync(new ShardedGuildCountObject
            {
                ShardId = startIndex,
                ShardCount = shardCount,
                Shards = shards
            });
        }

        /// <summary>
        /// Sends a request to update your stats by guild and shard count.
        /// </summary>
        /// <param name="guildCount">Represents the current guild count for the bot.</param>
        /// <param name="shardCount">Represents the current shard count that the bot uses.</param>
        public async Task UpdateStatsAsync(int guildCount, int shardCount)
        {
            await UpdateStatsInternalAsync(new ShardedGuildCountObject
            {
                ShardCount = shardCount,
                GuildCount = guildCount
            });
        }

        /// <summary>
        /// Returns a bool that defines if the specified user has voted for this bot within the last 12 hours.
        /// </summary>
        /// <param name="userId">Represents the ID of the user that you wish to check.</param>
        /// <returns>A bool that defines if a specific user has voted.</returns>
        public async Task<bool> HasVotedAsync(ulong userId)
            => await GetUserVoteAsync(userId);

        protected async Task<List<T>> GetVotersAsync<T>()
        {
            var query = $"bots/{_id}/votes";
            return await GetAuthorizedAsync<List<T>>(Utils.CreateQuery(query));
        }

        protected async Task UpdateStatsInternalAsync(object stats)
        {
            string json = JsonConvert.SerializeObject(stats);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{baseEndpoint}/bots/{_id}/stats", content);
        }

        /// <summary>
		/// Returns the object of a custom authorized request with a provided URL.
		/// </summary>
		/// <typeparam name="T">Represents the type that this request will be cast to.</typeparam>
		/// <param name="url">Represents the partial URL that will be used for this call.</param>
		/// <returns>An object of type T from the specified URL.</returns>
        protected async Task<T> GetAuthorizedAsync<T>(string url)
            => await GetAsync<T>(url);

        protected async Task<bool> GetUserVoteAsync(ulong userId)
        {
            string url = $"bots/{_id}/check?userId={userId}";
            return (await GetAsync<VoteObject>(url)).Voted;
        }
    }
}
