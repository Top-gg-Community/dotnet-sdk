using DiscordBotsList.Api.Objects;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    internal class User : Entity, IDblUser
    {
        [JsonPropertyName("bio")]
        //   [JsonInclude]
        internal string biography;

        [JsonPropertyName("banner")]
        [JsonInclude]
        internal string bannerUrl;

        [JsonPropertyName("social")]
        [JsonInclude]
        internal SocialConnections social;

        [JsonPropertyName("color")]
        [JsonInclude]
        internal string color;

        [JsonPropertyName("supporter")]
        [JsonInclude]
        internal bool isSupporter;

        [JsonPropertyName("certifiedDev")]
        [JsonInclude]
        internal bool isCertified;

        [JsonPropertyName("mod")]
        [JsonInclude]
        internal bool isModerator;

        [JsonPropertyName("webMod")]
        [JsonInclude]
        internal bool isWebModerator;

        [JsonPropertyName("admin")]
        [JsonInclude]
        internal bool isAdmin;

        [JsonInclude]
        public string Biography => biography;

        public string BannerUrl => bannerUrl;

        public SocialConnections Connections => social;
        public string Color => color;
        public bool IsSupporter => isSupporter;
        public bool IsCertified => isCertified;
        public bool IsModerator => isModerator;
        public bool IsWebModerator => isWebModerator;
        public bool IsAdmin => isAdmin;

        //    internal string BannerUrl1 { get => bannerUrl1; set => bannerUrl1 = value; }
    }
}