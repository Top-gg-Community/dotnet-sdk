using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Objects
{
    public class SocialConnections
    {
        [JsonPropertyName("youtube")]
        public string YouTubeChannelId { get; internal set; }

        [JsonPropertyName("reddit")]
        public string RedditName { get; internal set; }

        [JsonPropertyName("twitter")]
        public string TwitterName { get; internal set; }

        [JsonPropertyName("instagram")]
        public string InstagramName { get; internal set; }

        [JsonPropertyName("github")]
        public string GitHubName { get; internal set; }
    }
}