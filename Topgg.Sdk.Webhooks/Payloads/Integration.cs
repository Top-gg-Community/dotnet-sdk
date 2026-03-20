using System.Text.Json.Serialization;
using Topgg.Sdk.Webhooks.Data;
using Topgg.Sdk.Webhooks.Serialization;

namespace Topgg.Sdk.Webhooks.Payloads;

/// <summary>An `integration.create` webhook payload.</summary>
public class IntegrationCreatePayload
{
    /// <summary>The unique identifier for this connection.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong ConnectionId { get; internal init; }

    /// <summary>The secret used to verify future webhook deliveries.</summary>
    [JsonPropertyName("webhook_secret")]
    public string Secret { get; internal init; }

    /// <summary>The project that the integration refers to.</summary>
    public PartialProject Project { get; internal init; }

    /// <summary>The user who triggered this event.</summary>
    public User User { get; internal init; }
}

/// <summary>An `integration.delete` webhook payload.</summary>
public class IntegrationDeletePayload
{
    /// <summary>The unique identifier for this connection.</summary>
    [JsonConverter(typeof(ULongToStringConverter))]
    public ulong ConnectionId { get; internal init; }
}