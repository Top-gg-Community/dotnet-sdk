using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal.Queries
{
	internal class BotSearchQuery : ISearchResult<IDblBot>
	{
		public IEnumerable<IDblBot> Items => results;

		public int CurrentPage => (int)Math.Ceiling(Offset / (double)Limit);

		public int ItemsPerPage => Limit;

		public int ItemCount => Total;

		public int PageCount => (int)Math.Ceiling(Limit / (double)Count);

		[JsonProperty("results")]
		internal List<DblBot> results { get; set; }

		[JsonProperty("limit")]
		public int Limit { get; internal set; }

		[JsonProperty("offset")]
		public int Offset { get; internal set; }

		[JsonProperty("count")]
		public int Count { get; internal set; }

		[JsonProperty("total")]
		public int Total { get; internal set; }
	}
}
