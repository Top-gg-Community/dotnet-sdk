using Newtonsoft.Json;
using System.Collections.Generic;
using DiscordBotList.Models;

namespace DiscordBotList.Internal
{
	internal class BotSearchResult : ISearchResult<IDblBot>
	{
        [JsonProperty("results")]
        public List<DblBot> Values { get; internal set; }

        IReadOnlyList<IDblBot> ISearchResult<IDblBot>.Values => Values;

        /// <inheritdoc />
        [JsonProperty("limit")]
        public int PageSize { get; internal set; }

        [JsonProperty("offset")]
        public int Offset { get; internal set; }

        [JsonProperty("count")]
        public int Count { get; internal set; }

        /// <inheritdoc />
        public int PageIndex => Utils.GetCeiling(Offset, PageSize);

        /// <inheritdoc />
        public int PageCount => Utils.GetCeiling(PageSize, Count);

        /// <inheritdoc />
        [JsonProperty("total")]
        public int ResultCount { get; internal set; }
    }
}
