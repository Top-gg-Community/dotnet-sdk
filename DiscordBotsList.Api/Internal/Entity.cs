using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class Entity : IDblEntity
    {
        public ulong Id => id;
        public string Username => username;
        public string Discriminator => discriminator;

        public string AvatarUrl => !string.IsNullOrEmpty(avatarUrl) ?
            $"https://cdn.discordapp.com/{ Id }/{avatarUrl}.png" :
            defaultAvatarUrl;

        [JsonPropertyName("id")]
        internal ulong id;

        [JsonPropertyName("username")]
        internal string username;

        [JsonPropertyName("discrminator")]
        internal string discriminator;

        [JsonPropertyName("avatar")]
        internal string avatarUrl;

        [JsonPropertyName("defAvatar")]
        internal string defaultAvatarUrl;

        public override string ToString()
        {
            return $"{username}#{discriminator}";
        }
    }
}