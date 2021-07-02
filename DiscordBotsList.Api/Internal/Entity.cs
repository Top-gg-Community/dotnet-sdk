using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    public class Entity : IDblEntity
    {
        public string AvatarUrl => !string.IsNullOrEmpty(Avatar) ?
            $"https://cdn.discordapp.com/{ Id }/{Avatar}.png" :
            DefaultAvatar;

        [JsonPropertyName("id")]
        public ulong Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("discrminator")]
        public string Discriminator { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("defAvatar")]
        public string DefaultAvatar { get; set; }

        public override string ToString()
        {
            return $"{Username}#{Discriminator}";
        }
    }
}