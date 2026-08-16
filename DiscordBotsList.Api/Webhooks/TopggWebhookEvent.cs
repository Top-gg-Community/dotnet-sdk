using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     A webhook event delivered by Top.gg. The <see cref="Type" /> field is one of
    ///     <see cref="TopggWebhookEvent.VoteCreateType" /> or
    ///     <see cref="TopggWebhookEvent.WebhookTestType" />; the strongly typed payload is
    ///     available through <see cref="GetVoteData" /> or <see cref="GetWebhookTestData" />.
    /// </summary>
    public class TopggWebhookEvent
    {
        /// <summary>Fired when a user votes for your project.</summary>
        public const string VoteCreateType = "vote.create";

        /// <summary>Fired when you send a test event from the Top.gg dashboard.</summary>
        public const string WebhookTestType = "webhook.test";

        /// <summary>The event type. Always <c>vote.create</c> or <c>webhook.test</c>.</summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>The raw event payload.</summary>
        [JsonPropertyName("data")]
        public JsonElement Data { get; set; }

        /// <summary>Whether this event is a <c>vote.create</c> event.</summary>
        public bool IsVoteCreate => Type == VoteCreateType;

        /// <summary>Whether this event is a <c>webhook.test</c> event.</summary>
        public bool IsWebhookTest => Type == WebhookTestType;

        /// <summary>The strongly typed payload of a <c>vote.create</c> event.</summary>
        public VoteWebhookData GetVoteData()
        {
            return DeserializeData<VoteWebhookData>();
        }

        /// <summary>The strongly typed payload of a <c>webhook.test</c> event.</summary>
        public WebhookTestData GetWebhookTestData()
        {
            return DeserializeData<WebhookTestData>();
        }

        private T DeserializeData<T>()
        {
            return Data.ValueKind == JsonValueKind.Object
                ? Data.Deserialize<T>(JsonDefaults.Options)
                : default;
        }
    }

    /// <summary>The payload of a <c>vote.create</c> webhook event.</summary>
    public class VoteWebhookData
    {
        /// <summary>The Top.gg vote ID (snowflake).</summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong Id { get; set; }

        /// <summary>
        ///     Vote weight (<c>1</c> normally, <c>2</c> during the weekend multiplier).
        /// </summary>
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        /// <summary>ISO 8601 timestamp of when the vote was cast.</summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of when the user can vote again.</summary>
        [JsonPropertyName("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>The project that received the vote.</summary>
        [JsonPropertyName("project")]
        public WebhookProject Project { get; set; }

        /// <summary>Parsed query parameters appended to the vote page URL.</summary>
        [JsonPropertyName("query")]
        public Dictionary<string, string> Query { get; set; }

        /// <summary>The user who cast the vote.</summary>
        [JsonPropertyName("user")]
        public WebhookUser User { get; set; }
    }

    /// <summary>The payload of a <c>webhook.test</c> event.</summary>
    public class WebhookTestData
    {
        /// <summary>The project the test event was sent for.</summary>
        [JsonPropertyName("project")]
        public WebhookProject Project { get; set; }

        /// <summary>The user who triggered the test.</summary>
        [JsonPropertyName("user")]
        public WebhookUser User { get; set; }
    }

    /// <summary>The project referenced by a webhook payload.</summary>
    public class WebhookProject
    {
        /// <summary>The Top.gg project ID.</summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong Id { get; set; }

        /// <summary>Project type (<c>bot</c> or <c>server</c>).</summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Type { get; set; }

        /// <summary>Platform (always <c>discord</c>).</summary>
        [JsonPropertyName("platform")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Platform { get; set; }

        /// <summary>Discord client ID of the bot or guild ID of the server.</summary>
        [JsonPropertyName("platform_id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong PlatformId { get; set; }
    }

    /// <summary>The user referenced by a webhook payload.</summary>
    public class WebhookUser
    {
        /// <summary>The user's Top.gg ID.</summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong Id { get; set; }

        /// <summary>The user's platform-specific ID (for example their Discord user ID).</summary>
        [JsonPropertyName("platform_id")]
        [JsonConverter(typeof(ULongToStringConverter))]
        public ulong PlatformId { get; set; }

        /// <summary>The user's display name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>The user's avatar URL.</summary>
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
