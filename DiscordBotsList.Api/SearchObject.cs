using DiscordBotList.Api;

namespace DiscordBotsList.Api
{
    public class SearchObject
	{
		public DblField Field { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return $"{Field.ToString().ToLower()}: {Value}";
		}
	}
}
