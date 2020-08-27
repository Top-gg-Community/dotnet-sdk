using DiscordBotList.Internal;

namespace DiscordBotList.Widgets
{
    /// <summary>
    /// Represents a builder for an <see cref="IDblWidget"/> of <see cref="WidgetSize.Large"/>.
    /// </summary>
	public class LargeWidgetBuilder
	{
        /// <summary>
        /// Gets or sets the raw color value for the top panel of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
		public int? TopColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the middle panel of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        [QueryParam(format: "X")]
        public int? MiddleColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the username of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        [QueryParam(format: "X")]
        public int? UsernameColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the certification panel of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
        public int? CertifiedColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the data of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
        public int? DataColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the label of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
        public int? LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the highlight of this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
        public int? HighlightColor { get; set; }

        /// <summary>
        /// Sets the top panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithTopColor(int rgb)
        {
            TopColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the top panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
        public LargeWidgetBuilder WithTopColor(byte r, byte g, byte b)
		{
			TopColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the middle panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithMiddleColor(int rgb)
        {
            MiddleColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the middle panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
        public LargeWidgetBuilder WithMiddleColor(byte r, byte g, byte b)
		{
			MiddleColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the username color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithUsernameColor(int rgb)
        {
            UsernameColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the username color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public LargeWidgetBuilder WithUsernameColor(byte r, byte g, byte b)
		{
			UsernameColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the certified panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithCertifiedColor(int rgb)
        {
            CertifiedColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the certified panel color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public LargeWidgetBuilder WithCertifiedColor(byte r, byte g, byte b)
		{
			CertifiedColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the data color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithDataColor(int rgb)
        {
            DataColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the data color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public LargeWidgetBuilder WithDataColor(byte r, byte g, byte b)
		{
			DataColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the label color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithLabelColor(int rgb)
        {
            LabelColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the label color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public LargeWidgetBuilder WithLabelColor(byte r, byte g, byte b)
		{
			LabelColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the highlight color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
        public LargeWidgetBuilder WithHighlightColor(int rgb)
        {
            HighlightColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the highlight color for this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public LargeWidgetBuilder WithHighlightColor(int r, int g, int b)
		{
			HighlightColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Builds a new <see cref="IDblWidget"/> from this <see cref="LargeWidgetBuilder"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the bot to build a widget for.</param>
        /// <param name="format">The image format that this <see cref="IDblWidget"/> will be built for.</param>
        /// <returns>A generic <see cref="IDblWidget"/> object.</returns>
        public IDblWidget Build(ulong id, WidgetImageFormat format = WidgetImageFormat.Svg)
            => new DblWidget(id, format, this);

        /// <summary>
        /// Returns a string that represents the image URL of an <see cref="IDblWidget"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the bot to build a widget for.</param>
        /// <param name="format">The image format that this <see cref="IDblWidget"/> will be built for.</param>
        /// <returns>An image URL representing a widget.</returns>
        public string ToString(ulong id, WidgetImageFormat format = WidgetImageFormat.Svg)
            => DblApi.GetWidgetUrl(id, format, TopColor, MiddleColor,
                UsernameColor, CertifiedColor, DataColor, LabelColor, HighlightColor);
	}
}
