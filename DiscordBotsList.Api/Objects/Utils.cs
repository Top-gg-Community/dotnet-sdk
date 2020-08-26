namespace DiscordBotsList.Api.Objects
{
    internal static class Utils
	{
		public static int FromColor(float r, float g, float b)
			=> FromColor((int)(r * 255), (int)(g * 255), (int)(b * 255));

		public static int FromColor(int r, int g, int b)
			=> (255 << 24) | ((byte)r << 16) | ((byte)g << 8) | ((byte)b << 0);

		public static string CreateQuery(string baseUrl, params string[] args)
		{
			if (args.Length > 0)
			{
				return $"{baseUrl}?{string.Join("&", args)}";
			}
			return baseUrl;
		}
	}
}
