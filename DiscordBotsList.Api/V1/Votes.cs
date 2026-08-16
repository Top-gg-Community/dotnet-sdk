using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     The ID namespace to use when looking up a user's vote status. When querying
    ///     by Discord ID use <see cref="Discord" />, otherwise the Top.gg Snowflake ID
    ///     is assumed.
    /// </summary>
    public enum VoteSource
    {
        /// <summary>The user's Top.gg Snowflake ID (default).</summary>
        Topgg,

        /// <summary>The user's Discord Snowflake ID.</summary>
        Discord,
    }

    /// <summary>
    ///     A cursor-paginated page of votes returned by <c>GET /projects/@me/votes</c>.
    ///     Pass <see cref="Cursor" /> back to the API to fetch the next page.
    /// </summary>
    public class VotePage
    {
        /// <summary>Opaque cursor string for fetching the next page.</summary>
        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }

        /// <summary>Array of vote records in this page.</summary>
        [JsonPropertyName("data")]
        public List<Vote> Data { get; set; }

        /// <summary>Whether this page contains no votes.</summary>
        public bool IsEmpty => Data is null || Data.Count == 0;
    }

    /// <summary>A single vote record.</summary>
    public class Vote
    {
        /// <summary>The Top.gg user ID of the voter.</summary>
        [JsonPropertyName("userId")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong UserId { get; set; }

        /// <summary>
        ///     The voter's platform-specific user ID (for example their Discord user ID).
        /// </summary>
        [JsonPropertyName("platformId")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong PlatformId { get; set; }

        /// <summary>
        ///     The number of votes this single action counted for (for example <c>2</c>
        ///     on weekends for Discord bots).
        /// </summary>
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        /// <summary>ISO 8601 timestamp of when the user voted.</summary>
        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of when the user is eligible to vote again.</summary>
        [JsonPropertyName("expiresAt")]
        public DateTimeOffset ExpiresAt { get; set; }
    }

    /// <summary>
    ///     The most recent vote status of a single user, returned by
    ///     <c>GET /projects/@me/votes/:user_id</c>. A <c>404</c> response (the user has
    ///     never voted or their vote has expired) surfaces as <c>null</c> from the client.
    /// </summary>
    public class VoteStatus
    {
        /// <summary>ISO 8601 timestamp of when the user last voted.</summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        ///     ISO 8601 timestamp of when the user can vote again. If the current time is
        ///     before this value, the user has an active vote.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>The number of votes the user's last vote counted for.</summary>
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        /// <summary>Whether the user currently has an active vote.</summary>
        public bool IsActive => DateTimeOffset.UtcNow < ExpiresAt;
    }
}
