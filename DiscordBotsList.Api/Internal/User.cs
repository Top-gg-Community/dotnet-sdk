using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    public class User : Entity, IDblUser
    {
        [JsonPropertyName("social")] public SocialConnections Social { get; set; }
        
        [JsonPropertyName("bio")] public string Biography { get; set; }

        [JsonPropertyName("banner")] public string BannerUrl { get; set; }

        [JsonPropertyName("color")] public string Color { get; set; }

        [JsonPropertyName("supporter")] public bool IsSupporter { get; set; }

        [JsonPropertyName("certifiedDev")] public bool IsCertified { get; set; }

        [JsonPropertyName("mod")] public bool IsModerator { get; set; }

        [JsonPropertyName("webMod")] public bool IsWebModerator { get; set; }

        [JsonPropertyName("admin")] public bool IsAdmin { get; set; }

        public SocialConnections Connections => Social;
    }
}