using System.Collections.Generic;
using System.Text.Json.Serialization;
using Topgg.Sdk.Api.Serialization;

namespace Topgg.Sdk.Api.Data;

/// <summary>A project's platform.</summary>
public enum Platform
{
    Discord
}

/// <summary>A project's type.</summary>
public enum ProjectType
{
    Bot,
    Server
}

/// <summary>A project listed on Top.gg.</summary>
public class Project
{
    /// <summary>The project's ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong Id { get; internal init; }

    /// <summary>The project's name sourced from the external platform.</summary>
    public string Name { get; internal init; }

    /// <summary>The project's platform.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Platform Platform { get; internal init; }

    /// <summary>The project's type.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectType Type { get; internal init; }

    /// <summary>The project's short description.</summary>
    public string Headline { get; internal init; }

    /// <summary>The project's tag IDs.</summary>
    public List<string> Tags { get; internal init; }

    /// <summary>The project's current vote count that affects the project's ranking.</summary>
    public int Votes { get; internal init; }

    /// <summary>The project's total vote count.</summary>
    [JsonPropertyName("votes_total")]
    public int TotalVotes { get; internal init; }

    /// <summary>The project's review score out of 5.</summary>
    public float ReviewScore { get; internal init; }

    /// <summary>The project's total review count.</summary>
    public int ReviewCount { get; internal init; }
}

/// <summary>A brief information on a project listed on Top.gg.</summary>
public class PartialProject
{
    /// <summary>The project's ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong Id { get; internal init; }

    /// <summary>The project's type.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectType Type { get; internal init; }

    /// <summary>The project's platform.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Platform Platform { get; internal init; }

    /// <summary>The project's platform ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong PlatformId { get; internal init; }

}
