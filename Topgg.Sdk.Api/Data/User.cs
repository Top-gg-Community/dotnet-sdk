using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Topgg.Sdk.Api.Serialization;

namespace Topgg.Sdk.Api.Data;

/// <summary>A user account from an external platform that is linked to a Top.gg user account.</summary>
public enum UserSource
{
    Discord,
    Topgg,
}

/// <summary>A brief information of a project's vote.</summary>
public class PartialVote
{
    /// <summary>When the vote was cast.</summary>
    [JsonPropertyName("created_at")]
    public DateTime VotedAt { get; internal init; }

    /// <summary>When the vote expires and the user is required to vote again.</summary>
    public DateTime ExpiresAt { get; internal init; }

    /// <summary>The number of votes this vote counted for. This is a rounded integer value which determines how many points this individual vote was worth.</summary>
    public int Weight { get; internal init; }
}

/// <summary>A project's vote information.</summary>
public class Vote : PartialVote
{
    /// <summary>The voter's ID.</summary>
    [JsonPropertyName("user_id")]
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong VoterId { get; internal init; }

    /// <summary>The voter's ID on the project's platform.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong PlatformId { get; internal init; }
}

/// <summary>A paginated list of a project's vote information.</summary>
public class PaginatedVotes
{
    /// <summary>The votes in this page.</summary>
    [JsonPropertyName("data")]
    public List<Vote> Votes { get; internal init; }

    [JsonInclude]
    internal string Cursor { get; init; }

    internal TopggApi Client;

    /// <summary>Tries to advance to the next page.</summary>
    /// <returns>The next page of votes.</returns>
    public async Task<PaginatedVotes> Next() => await Client.GetVotesAsync(Cursor);
}