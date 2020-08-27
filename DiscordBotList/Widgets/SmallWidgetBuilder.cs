using DiscordBotList.Internal;

namespace DiscordBotList.Widgets
{
	/// <summary>
	/// Represents a builder for an <see cref="IDblWidget"/> of <see cref="WidgetSize.Small"/>.
	/// </summary>
	public class SmallWidgetBuilder
	{
		/// <summary>
		/// Gets or sets the <see cref="SmallWidgetType"/> of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		public SmallWidgetType Type { get; set; }

		/// <summary>
		/// Gets or sets the avatar toggle of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
        [QueryParam(toLower: true)]
		public bool? UseAvatar { get; set; }

		/// <summary>
		/// Gets or sets the raw color value for the avatar background of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		[QueryParam("avatarbg", "X")]
		public int? AvatarBackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the raw color value for the left panel of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		[QueryParam(format: "X")]
		public int? LeftColor { get; set; }

		/// <summary>
		/// Gets or sets the raw color value for the right panel of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
        [QueryParam(format: "X")]
		public int? RightColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the left panel text of this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
		public int? LeftTextColor { get; set; }

        /// <summary>
        /// Gets or sets the raw color value for the right panel text of this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
		[QueryParam(format: "X")]
		public int? RightTextColor { get; set; }

		/// <summary>
		/// Sets the <see cref="SmallWidgetType"/> of this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="type">The new <see cref="SmallWidgetType"/> that this <see cref="SmallWidgetBuilder"/> will represent.</param>
        public SmallWidgetBuilder WithType(SmallWidgetType type)
		{
			Type = type;
			return this;
		}

        /// <summary>
        /// Sets the avatar background color for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
		public SmallWidgetBuilder WithAvatarBackgroundColor(int rgb)
        {
            AvatarBackgroundColor = rgb;
            return this;
        }

		/// <summary>
		/// Sets the avatar background color for this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="r">The R component of this color.</param>
		/// <param name="g">The G component of this color.</param>
		/// <param name="b">The B component of this color.</param>
		public SmallWidgetBuilder WithAvatarBackgroundColor(byte r, byte g, byte b)
		{
			AvatarBackgroundColor = Utils.FromColor(r, g, b);
			return this;
		}

		/// <summary>
        /// Sets the left panel color for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
		public SmallWidgetBuilder WithLeftColor(int rgb)
        {
            LeftColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the left panel color for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public SmallWidgetBuilder WithLeftColor(byte r, byte g, byte b)
		{
			LeftColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the right panel color for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="rgb">The raw color value to set.</param>
		public SmallWidgetBuilder WithRightColor(int rgb)
        {
            RightColor = rgb;
            return this;
        }

        /// <summary>
        /// Sets the right panel color for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="r">The R component of this color.</param>
        /// <param name="g">The G component of this color.</param>
        /// <param name="b">The B component of this color.</param>
		public SmallWidgetBuilder WithRightColor(byte r, byte g, byte b)
		{
			RightColor = Utils.FromColor(r, g, b);
			return this;
		}

		/// <summary>
		/// Sets the left panel text color for this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="rgb">The raw color value to set.</param>
		public SmallWidgetBuilder WithLeftTextColor(int rgb)
        {
            LeftTextColor = rgb;
            return this;
        }

		/// <summary>
		/// Sets the left panel text color for this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="r">The R component of this color.</param>
		/// <param name="g">The G component of this color.</param>
		/// <param name="b">The B component of this color.</param>
		public SmallWidgetBuilder WithLeftTextColor(byte r, byte g, byte b)
		{
			LeftTextColor = Utils.FromColor(r, g, b);
			return this;
		}

		/// <summary>
		/// Sets the right panel text color for this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="rgb">The raw color value to set.</param>
		public SmallWidgetBuilder WithRightTextColor(int rgb)
        {
            RightTextColor = rgb;
            return this;
        }

		/// <summary>
		/// Sets the right panel text color for this <see cref="SmallWidgetBuilder"/>.
		/// </summary>
		/// <param name="r">The R component of this color.</param>
		/// <param name="g">The G component of this color.</param>
		/// <param name="b">The B component of this color.</param>
		public SmallWidgetBuilder WithRightTextColor(byte r, byte g, byte b)
		{
			RightTextColor = Utils.FromColor(r, g, b);
			return this;
		}

        /// <summary>
        /// Sets the avatar display toggle for this <see cref="SmallWidgetBuilder"/>.
        /// </summary>
        /// <param name="toggle">The toggle state for the avatar display.</param>
		public SmallWidgetBuilder WithAvatarToggle(bool toggle)
        {
            UseAvatar = toggle;
            return this;
        }

		/// <summary>
		/// Builds a new <see cref="IDblWidget"/> from this <see cref="SmallWidgetBuilder"/>.
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
		    => DblApi.GetWidgetUrl(id, Type, format, AvatarBackgroundColor,
                LeftColor, RightColor, LeftTextColor, RightTextColor, UseAvatar ?? true);
	}
}
