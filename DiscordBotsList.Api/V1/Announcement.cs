using System;
using System.Text.Json.Serialization;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     Request body for <c>POST /projects/@me/announcements</c>.
    /// </summary>
    public class AnnouncementRequest
    {
        /// <summary>The announcement title. Must be between 3 and 100 characters.</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>The announcement body text. Must be between 10 and 2,000 characters.</summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        ///     The category to publish the announcement under. One of <c>announcement</c>,
        ///     <c>event</c>, or <c>new_feature</c>. Defaults to <c>announcement</c> when omitted.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    /// <summary>An announcement created for a project.</summary>
    public class Announcement
    {
        /// <summary>The title of the created announcement.</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>The body content of the created announcement.</summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>The category the announcement was published under.</summary>
        [JsonPropertyName("category")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Category { get; set; }

        /// <summary>ISO 8601 timestamp of when the announcement was created.</summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
