using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    public class Entity : IDblEntity
    {
        public ulong Id => id;
        public string Username => username;
        public string Discriminator => discriminator;

        public string AvatarUrl => !string.IsNullOrEmpty(avatarUrl) ?
            $"https://cdn.discordapp.com/{ Id }/{avatarUrl}.png" :
            defaultAvatarUrl;

        [JsonPropertyName("id")]
        [JsonInclude]
        internal ulong id;

        [JsonPropertyName("username")]
        [JsonInclude]
        internal string username;

        [JsonPropertyName("discrminator")]
        [JsonInclude]
        internal string discriminator;

        [JsonPropertyName("avatar")]
        [JsonInclude]
        internal string avatarUrl;

        [JsonPropertyName("defAvatar")]
        [JsonInclude]
        internal string defaultAvatarUrl;

        public override string ToString()
        {
            return $"{username}#{discriminator}";
        }
    }
}