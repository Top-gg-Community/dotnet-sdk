namespace DiscordBotList.Widgets
{
    /// <summary>
    /// Represents a generic widget from the Discord Bot List API.
    /// </summary>
    public interface IDblWidget
    {
        /// <summary>
        /// Represents the unique identifier of the user that this <see cref="IDblWidget"/> represents.
        /// </summary>
        ulong UserId { get; }

        /// <summary>
        /// Represents the image format for this <see cref="IDblWidget"/>.
        /// </summary>
        WidgetImageFormat Format { get; }

        /// <summary>
        /// Represents the size of this <see cref="IDblWidget"/>.
        /// </summary>
        WidgetSize Size { get; }

        /// <summary>
        /// Represents the image URL of this <see cref="IDblWidget"/>.
        /// </summary>
        string ImageUrl { get; }
    }
}
