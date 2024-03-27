using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    public class Entity : IDblEntity
    {
        [JsonPropertyName("avatar")] public string Avatar { get; set; }

        [JsonPropertyName("defAvatar")] public string DefaultAvatar { get; set; }

        public string AvatarUrl => !string.IsNullOrEmpty(Avatar)
            ? $"https://cdn.discordapp.com/{Id}/{Avatar}.png"
            : $"https://cdn.discordapp.com/{Id}/{DefaultAvatar}.png";

        [JsonPropertyName("id")] public ulong Id { get; set; }

        [JsonPropertyName("username")] public string Username { get; set; }

        [JsonPropertyName("discriminator")] public string Discriminator { get; set; }

        public override string ToString()
        {
            return $"{Username}#{Discriminator}";
        }
    }
}