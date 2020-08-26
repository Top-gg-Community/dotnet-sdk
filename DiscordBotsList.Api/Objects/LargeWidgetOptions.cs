using DiscordBotsList.Api.Internal;

namespace DiscordBotsList.Api.Objects
{
    public class LargeWidgetOptions
	{
        public WidgetImageFormat Format { get; set; } = WidgetImageFormat.Svg;

		[QueryParam(format: "X")]
		public int? TopColor { get; set; }

        [QueryParam(format: "X")]
        public int? MiddleColor { get; set; }

        [QueryParam(format: "X")]
        public int? UsernameColor { get; set; }

		[QueryParam(format: "X")]
        public int? CertifiedColor { get; set; }

		[QueryParam(format: "X")]
        public int? DataColor { get; set; }

		[QueryParam(format: "X")]
        public int? LabelColor { get; set; }

		[QueryParam(format: "X")]
        public int? HighlightColor { get; set; }

		public LargeWidgetOptions SetTopColor(int r, int g, int b)
		{
			TopColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetMiddleColor(int r, int g, int b)
		{
			MiddleColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetUsernameColor(int r, int g, int b)
		{
			UsernameColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetCertifiedColor(int r, int g, int b)
		{
			CertifiedColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetDataColor(int r, int g, int b)
		{
			DataColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetLabelColor(int r, int g, int b)
		{
			LabelColor = Utils.FromColor(r, g, b);
			return this;
		}
		public LargeWidgetOptions SetHighlightColor(int r, int g, int b)
		{
			HighlightColor = Utils.FromColor(r, g, b);
			return this;
		}

        public string Build(ulong id)
            => DblApi.GetWidgetUrl(id, Format, TopColor, MiddleColor,
                UsernameColor, CertifiedColor, DataColor, LabelColor, HighlightColor);
    }
}
