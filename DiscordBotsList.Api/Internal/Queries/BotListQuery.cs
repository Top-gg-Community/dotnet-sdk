using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBotsList.Api.Internal.Queries
{
	internal class BotListQuery : ISearchResult<IBot>
	{
		public List<IBot> Items => results
			.Cast<IBot>()
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
