using System.Text.Json.Serialization;
using Topgg.Sdk.Webhooks.Serialization;

namespace Topgg.Sdk.Webhooks.Data;

/// <summary>A Top.gg user.</summary>
public class User
{
    /// <summary>The user's ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong Id { get; internal init; }

    /// <summary>The user's name.</summary>
    public string Name { get; internal init; }

    /// <summary>The user's avatar URL.</summary>
    [JsonPropertyName("avatar_url")]
    public string Avatar { get; internal init; }

    /// <summary>The user's platform ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong PlatformId { get; internal init; }
}