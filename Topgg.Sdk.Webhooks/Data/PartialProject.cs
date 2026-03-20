using System.Text.Json.Serialization;
using Topgg.Sdk.Webhooks.Serialization;

namespace Topgg.Sdk.Webhooks.Data;

/// <summary>A brief information on project listed on Top.gg.</summary>
public class PartialProject
{
    /// <summary>The project's ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong Id { get; internal init; }

    /// <summary>The project's ID.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectType Type { get; internal init; }

    /// <summary>The project's platform.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Platform Platform { get; internal init; }

    /// <summary>The project's platform ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong PlatformId { get; internal init; }
}