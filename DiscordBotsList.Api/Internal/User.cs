using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class User : Entity, IDblUser
    {
        [JsonPropertyName("bio")]
        internal string biography;

        [JsonPropertyName("banner")]
        internal string bannerUrl;

        [JsonPropertyName("social")]
        internal SocialConnections social;

        [JsonPropertyName("color")]
        internal string color;

        [JsonPropertyName("supporter")]
        internal bool isSupporter;

        [JsonPropertyName("certifiedDev")]
        internal bool isCertified;

        [JsonPropertyName("mod")]
        internal bool isModerator;

        [JsonPropertyName("webMod")]
        internal bool isWebModerator;

        [JsonPropertyName("admin")]
        internal bool isAdmin;

        public string Biography => biography;
        public string BannerUrl => bannerUrl;
        public SocialConnections Connections => social;
        public string Color => color;
        public bool IsSupporter => isSupporter;
        public bool IsCertified => isCertified;
        public bool IsModerator => isModerator;
        public bool IsWebModerator => isWebModerator;
        public bool IsAdmin => isAdmin;
    }
}