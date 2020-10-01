using DiscordBotList.Models;
using Newtonsoft.Json;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblUser"/>
	internal class DblUser : DblEntity, IDblUser
	{
        /// <inheritdoc />
        [JsonProperty("supporter")]
		public bool IsSupporter { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("certifiedDev")]
		public bool IsCertified { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("mod")]
		public bool IsModerator { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("webMod")]
		public bool IsWebModerator { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("admin")]
		public bool IsAdmin { get; internal set; }

        /// <inheritdoc />
        public DblStatus Status => CreateStatus(IsSupporter, IsCertified, IsModerator, IsWebModerator, IsAdmin);

        /// <inheritdoc />
        [JsonProperty("social")]
        public SocialConnections Connections { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("bio")]
        public string Biography { get; internal set; }

        /// <inheritdoc />
        public string PageUrl => DblApi.GetUserUrl(Id);

        /// <inheritdoc />
        [JsonProperty("banner")]
        public string BannerUrl { get; internal set; }

        /// <inheritdoc />
        [JsonProperty("color")]
        public string ColorHex { get; internal set; }

        /// <inheritdoc />
        public int RawColor => Utils.GetColorValue(ColorHex);

        private static DblStatus CreateStatus(bool isSupporter, bool isCertified, bool isModerator, bool isWebModerator, bool isAdmin)
        {
            DblStatus status = 0;

            if (isSupporter)
                status |= DblStatus.Supporter;

            if (isCertified)
                status |= DblStatus.Certified;

            if (isModerator)
                status |= DblStatus.Moderator;

            if (isWebModerator)
                status |= DblStatus.WebModerator;

            if (isAdmin)
                status |= DblStatus.Admin;

            return status;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"ID: {Id}\nName: {Username}\nStatus: {Status}\nPageUrl: {PageUrl}\nColorHex: {ColorHex}\nRawColor {RawColor}";
        }
    }
}
