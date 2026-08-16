using System.Collections.Generic;
using System.Text.Json.Serialization;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     The project associated with an authenticated Top.gg v1 token,
    ///     returned by <c>GET /projects/@me</c>.
    /// </summary>
    public class Project
    {
        /// <summary>The unique Top.gg-sourced Snowflake identifier for the project.</summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong Id { get; set; }

        /// <summary>The project name as it appears on the external platform.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///     The project type. One of <c>bot</c>, <c>server</c>, or <c>game</c>.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Type { get; set; }

        /// <summary>
        ///     The platform the project belongs to. One of <c>discord</c> or <c>roblox</c>.
        /// </summary>
        [JsonPropertyName("platform")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Platform { get; set; }

        /// <summary>The short description set during project creation.</summary>
        [JsonPropertyName("headline")]
        public string Headline { get; set; }

        /// <summary>List of tag IDs associated with the project.</summary>
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        /// <summary>Current vote count used for ranking calculations.</summary>
        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        /// <summary>Total all-time votes received by the project.</summary>
        [JsonPropertyName("votesTotal")]
        public int VotesTotal { get; set; }

        /// <summary>Average review score across all reviews.</summary>
        [JsonPropertyName("reviewScore")]
        public double? ReviewScore { get; set; }

        /// <summary>Total number of reviews submitted for the project.</summary>
        [JsonPropertyName("reviewCount")]
        public int? ReviewCount { get; set; }
    }

    /// <summary>
    ///     Locale-keyed content update for a project, sent to <c>PATCH /projects/@me</c>.
    ///     At least one of <see cref="Headline" /> or <see cref="PageContent" /> must be set.
    /// </summary>
    public class ProjectUpdate
    {
        public ProjectUpdate()
        {
        }

        public ProjectUpdate(Dictionary<string, string> headline = null, Dictionary<string, string> pageContent = null)
        {
            Headline = headline;
            PageContent = pageContent;
        }

        /// <summary>
        ///     A map of locale codes to headline strings. Each headline must be between
        ///     3 and 140 characters.
        /// </summary>
        [JsonPropertyName("headline")]
        public Dictionary<string, string> Headline { get; set; }

        /// <summary>
        ///     A map of locale codes to page content strings (Markdown supported). Each
        ///     entry must be between 300 and 50,000 characters.
        /// </summary>
        [JsonPropertyName("pageContent")]
        public Dictionary<string, string> PageContent { get; set; }
    }
}
