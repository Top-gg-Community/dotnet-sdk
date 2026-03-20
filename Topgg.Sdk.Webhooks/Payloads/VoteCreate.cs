using System;
using System.Text.Json.Serialization;
using Topgg.Sdk.Webhooks.Data;
using Topgg.Sdk.Webhooks.Serialization;

namespace Topgg.Sdk.Webhooks.Payloads;

/// <summary>A `vote.create` webhook payload.</summary>
public class VoteCreatePayload
{
    /// <summary>The vote's ID.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong Id { get; internal init; }

    /// <summary>The number of votes this vote counted for. This is a rounded integer value which determines how many points this individual vote was worth.</summary>
    public int Weight { get; internal init; }

    /// <summary>When the vote was cast.</summary>
    [JsonPropertyName("created_at")]
    public DateTime VotedAt { get; internal init; }

    /// <summary>When the vote expires (the user can vote again.)</summary>
    public DateTime ExpiresAt { get; internal init; }

    /// <summary>The project that received this vote.</summary>
    public PartialProject Project { get; internal init; }

    /// <summary>The user who voted for this project.</summary>
    public User User { get; internal init; }
}