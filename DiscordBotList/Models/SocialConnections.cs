using DiscordBotList.Internal;
using Newtonsoft.Json;

namespace DiscordBotList.Models
{
	/// <summary>
	/// Represents a collection of social media connections for an <see cref="IDblUser"/>.
	/// </summary>
	public class SocialConnections
	{
        /// <summary>
        /// Represents the channel ID of a YouTube account.
        /// </summary>
		[JsonProperty("youtube")]
		public string YouTubeChannelId { get; internal set; }

        /// <summary>
        /// Represents the username of a Reddit account.
        /// </summary>
		[JsonProperty("reddit")]
		public string RedditId { get; internal set; }

        /// <summary>
		/// Represents the username of a Twitter account.
		/// </summary>
		[JsonProperty("twitter")]
		public string TwitterId { get; internal set; }

        /// <summary>
        /// Represents the username of an Instagram account.
        /// </summary>
		[JsonProperty("instagram")]
		public string InstagramId { get; internal set; }

        /// <summary>
        /// Represents the username of a GitHub account.
        /// </summary>
		[JsonProperty("github")]
		public string GitHubId { get; internal set; }

        /// <summary>
        /// Gets the URL that points to a YouTube channel (if <see cref="YouTubeChannelId"/> was specified).
        /// </summary>
        public string YouTubeUrl => DblApi.GetYouTubeUrl(YouTubeChannelId);

        /// <summary>
        /// Gets the URL that points to a Reddit page (if <see cref="RedditId"/> was specified).
        /// </summary>
        public string RedditUrl => DblApi.GetRedditUrl(RedditId);

        /// <summary>
        /// Gets the URL that points to a Twitter page (if <see cref="TwitterId"/> was specified).
        /// </summary>
        public string TwitterUrl => DblApi.GetTwitterUrl(TwitterId);

        /// <summary>
        /// Gets the URL that points to an Instagram page (if <see cref="InstagramId"/> was specified).
        /// </summary>
        public string InstagramUrl => DblApi.GetInstagramUrl(InstagramId);

        /// <summary>
        /// Gets the URL that points to a GitHub page (if <see cref="GitHubId"/> was specified).
        /// </summary>
		public string GetGitHubUrl => DblApi.GetGitHubUrl(GitHubId);
    }
}
