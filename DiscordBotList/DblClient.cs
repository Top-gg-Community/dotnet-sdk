using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DiscordBotList.Internal;
using DiscordBotList.Models;
using Newtonsoft.Json;

namespace DiscordBotList
{
    /// <summary>
    /// Represents an authenticated Discord Bot List API client.
    /// </summary>
    public class DblClient : BaseDblClient
    {
        private readonly JsonSerializerSettings _statSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        };

        /// <summary>
        /// Initializes a new <see cref="DblClient"/> using the specified ID and token.
        /// </summary>
        /// <param name="id">The unique identifier of the bot that this <see cref="DblClient"/> will be bound to.</param>
        /// <param name="token">The token used to access the Discord Bot List API.</param>
        public DblClient(ulong id, string token)
        {
            CurrentUserId = id;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Gets the unique identifier for this currently logged-in user.
        /// </summary>
        public ulong CurrentUserId { get; }

        /// <summary>
        /// Returns an instance of the bot that this <see cref="DblClient"/> is bound to.
        /// </summary>
        /// <returns>An <see cref="IDblSelfBot"/> instance.</returns>
        public async Task<IDblSelfBot> GetSelfAsync()
        {
            var bot = await GetBotAsync<DblSelfBot>(CurrentUserId);
            bot.Client = this;
            return bot;
        }

        /// <summary>
        /// Returns a list of all users that have voted for your bot for up to 1,000 entries. If you have more than 1,000 total votes, you are required to use webhooks instead.
        /// </summary>
        /// <returns>A list of all users that have voted.</returns>
        public async Task<List<IDblEntity>> GetVotersAsync()
        {
            return (await GetVotersAsync<DblEntity>()).ToList<IDblEntity>();
        }

        /// <summary>
        /// Sends a request to update your stats by guild count.
        /// </summary>
        /// <param name="guildCount">Represents the current guild count for the bot.</param>
        public async Task UpdateStatsAsync(int guildCount)
        {
            await UpdateStatsInternalAsync(new StatsObject
            {
                GuildCount = guildCount
            });
        }

        /// <summary>
        /// Sends a request to update your stats with a collection of guild counts for each shard.
        /// </summary>
        /// <param name="shards">Represents a collection of guild counts for each shard, starting from the specified index.</param>
        public async Task UpdateStatsAsync(IEnumerable<int> shards)
        {
            int[] values = shards.ToArray();

            await UpdateStatsInternalAsync(new StatsObject
            {
                ShardId = 0,
                ShardCount = values.Length,
                Shards = values.ToArray()
            });
        }

        /// <summary>
        /// Sends a request to update your stats by specific shard values.
        /// </summary>
        /// <param name="startIndex">Represents the starting index at which to update your shards from.</param>
        /// <param name="shardCount">Represents the current shard count that the bot uses.</param>
        /// <param name="shards">Represents an array of guild counts for each shard, starting from the specified index.</param>
        public async Task UpdateStatsAsync(int startIndex, int shardCount, params int[] shards)
        {
            await UpdateStatsInternalAsync(new StatsObject
            {
                ShardId = startIndex,
                ShardCount = shardCount,
                Shards = shards
            });
        }

        /// <summary>
        /// Asynchronously sends a request to update your stats by guild and shard count.
        /// </summary>
        /// <param name="guildCount">Represents the current guild count for the bot.</param>
        /// <param name="shardCount">Represents the current shard count that the bot uses.</param>
        public async Task UpdateStatsAsync(int guildCount, int shardCount)
        {
            await UpdateStatsInternalAsync(new StatsObject
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
            => await GetAuthorizedAsync<List<T>>(DblApi.GetVotesEndpoint(CurrentUserId));

        protected async Task UpdateStatsInternalAsync(object stats)
        {
            string json = JsonConvert.SerializeObject(stats, _statSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await HttpClient.PostAsync($"{DblApi.BaseUrl}{DblApi.GetStatsEndpoint(CurrentUserId)}", content);
        }

        /// <summary>
		/// Returns the object of a custom authorized request with a provided endpoint.
		/// </summary>
        /// <typeparam name="T">The type that this request will be cast to.</typeparam>
        /// <param name="endpoint">The endpoint that will be appended to the base URL for this call.</param>
        /// <returns>An object of type T from the specified URL.</returns>
        protected async Task<T> GetAuthorizedAsync<T>(string endpoint)
            => await GetAsync<T>(endpoint);

        protected async Task<bool> GetUserVoteAsync(ulong userId)
            => (await GetAsync<VoteResult>(DblApi.GetVoteCheckEndpoint(CurrentUserId, userId))).Voted;
    }
}
