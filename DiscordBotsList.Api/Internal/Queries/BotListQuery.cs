using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal.Queries
{
    internal class BotListQuery : ISearchResult<IDblBot>
    {
        public List<IDblBot> Items => results
            .Cast<IDblBot>()
            .ToList();

        public int CurrentPage => (int)Math.Ceiling((double)offset / limit);

        public int ItemsPerPage => limit;

        public int TotalItems => total;

        public int TotalPages => (int)Math.Ceiling((double)limit / count);

        [JsonProperty("results")]
        public List<Bot> results;

        [JsonProperty("limit")]
        public int limit;

        [JsonProperty("offset")]
        public int offset;

        [JsonProperty("count")]
        public int count;

        [JsonProperty("total")]
        public int total;
    }
}