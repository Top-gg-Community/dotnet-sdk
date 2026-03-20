using System.Text.Json;

namespace Topgg.Sdk.Webhooks.Payloads;

internal class Payload
{
    public string Type { get; init; }

    public JsonElement Data { get; init; }
}