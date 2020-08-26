using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api.Objects
{
	public class SmallWidgetOptions
	{
		public SmallWidgetType Type { get; set; }

        public WidgetImageFormat Format { get; set; } = WidgetImageFormat.Svg;

        [QueryParam(toLower: true)]
		public bool? UseAvatar;

        [QueryParam("avatarbg", "X")]
		public int? AvatarBackgroundColor;

        [QueryParam(format: "X")]
		public int? LeftColor;

        [QueryParam(format: "X")]
		public int? RightColor;

        [QueryParam(format: "X")]
		public int? LeftTextColor;

        [QueryParam(format: "X")]
		public int? RightTextColor;

		public SmallWidgetOptions SetType(SmallWidgetType t)
		{
			Type = t;
			return this;
		}

		public SmallWidgetOptions SetAvatar(bool useAvatar)
		{
			UseAvatar = useAvatar;
			return this;
		}

		public SmallWidgetOptions SetAvatarBackgroundColor(int r, int g, int b)
		{
			AvatarBackgroundColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetLeftColor(int r, int g, int b)
		{
			LeftColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetRightColor(int r, int g, int b)
		{
			RightColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetLeftTextColor(int r, int g, int b)
		{
			LeftTextColor = Utils.FromColor(r, g, b);
			return this;
		}
		public SmallWidgetOptions SetRightTextColor(int r, int g, int b)
		{
			RightTextColor = Utils.FromColor(r, g, b);
			return this;
		}

		public SmallWidgetOptions SetUseAvatar(bool useAvatar)
		{
			UseAvatar = useAvatar;
			return this;
		}

        public string Build(ulong botId)
            => DblApi.GetWidgetUrl(botId, Type, Format, AvatarBackgroundColor,
                LeftColor, RightColor, LeftTextColor, RightTextColor, UseAvatar ?? true);
    }
}
