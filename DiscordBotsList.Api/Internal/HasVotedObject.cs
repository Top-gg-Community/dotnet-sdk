using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class HasVotedObject
    {
        [JsonPropertyName("voted")] public int? HasVoted { get; set; }
    }
}
