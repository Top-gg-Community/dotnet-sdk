using DiscordBotList.Api;
using System.Collections.Generic;

namespace DiscordBotsList.Api
{
    public class BotSearchParams
	{
		public int Limit { get; set; } = 500;
		public int Offset { get; set; } // amount of bots to skip
		public SortObject Sort { get; set; }
		public List<SearchObject> Search { get; set; }
		public DblField Fields { get; set; }
	}
}
