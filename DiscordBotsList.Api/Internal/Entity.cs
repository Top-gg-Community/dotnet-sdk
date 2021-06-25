using Newtonsoft.Json;
using DiscordBotsList.Api.Objects;

namespace DiscordBotsList.Api.Internal
{
    internal class Entity : IDblEntity
    {
        public ulong Id => id;
        public string Username => username;
        public string Discriminator => discriminator;

        public string AvatarUrl => !string.IsNullOrEmpty(avatarUrl) ?
            $"https://cdn.discordapp.com/{Id}/{avatarUrl}.png" :
            defaultAvatarUrl;

        [JsonProperty("id")]
        internal ulong id;

        [JsonProperty("username")]
        internal string username;

        [JsonProperty("discrminator")]
        internal string discriminator;

        [JsonProperty("avatar")]
        internal string avatarUrl;

        [JsonProperty("defAvatar")]
        internal string defaultAvatarUrl;

        public override string ToString()
        {
            return $"{username}#{discriminator}";
        }
    }
}