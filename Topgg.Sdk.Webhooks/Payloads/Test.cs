using Topgg.Sdk.Webhooks.Data;

namespace Topgg.Sdk.Webhooks.Payloads;

/// <summary>A `webhook.test` webhook payload.</summary>
public class TestPayload
{
    /// <summary>The project that the test refers to.</summary>=
    public PartialProject Project { get; internal init; }

    /// <summary>The user who triggered this test.</summary>=
    public User User { get; internal init; }
}