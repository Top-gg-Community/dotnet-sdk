using DiscordBotList.Widgets;

namespace DiscordBotList.Internal
{
    /// <inheritdoc cref="IDblWidget" />
    internal class DblWidget : IDblWidget
    {
        internal DblWidget(ulong id, WidgetImageFormat format, SmallWidgetBuilder builder)
            : this(id, format)
        {
            Size = WidgetSize.Small;
            ImageUrl = builder.ToString(id, format);
        }

        internal DblWidget(ulong id, WidgetImageFormat format, LargeWidgetBuilder builder)
            : this(id, format)
        {
            Size = WidgetSize.Small;
            ImageUrl = builder.ToString(id, format);
        }

        private DblWidget(ulong id, WidgetImageFormat format)
        {
            UserId = id;
            Format = format;
        }

        /// <inheritdoc />
        public ulong UserId { get; }

        /// <inheritdoc />
        public WidgetImageFormat Format { get; }

        /// <inheritdoc />
        public WidgetSize Size { get; }

        /// <inheritdoc />
        public string ImageUrl { get; }
    }
}
