namespace DiscordBotList.Widgets
{
    /// <summary>
    /// Defines the types that a <see cref="SmallWidgetBuilder"/> can represent.
    /// </summary>
    public enum SmallWidgetType
	{
        /// <summary>
        /// Marks the widget to display the user status of a bot.
        /// </summary>
        /// <remarks>
        /// An example can be found here: https://top.gg/api/widget/status/270904126974590976.svg
        /// </remarks>
        Status = 0,

        /// <summary>
        /// Marks the widget to display the total server count that a bot is connected to.
        /// </summary>
        /// <remarks>
        /// An example can be found here: https://top.gg/api/widget/servers/270904126974590976.svg
        /// </remarks>
        Servers = 1,

        /// <summary>
        /// Marks the widget to display the library that a bot was programmed in.
        /// </summary>
        /// <remarks>
        /// An example can be found here: https://top.gg/api/widget/lib/270904126974590976.svg
        /// </remarks>
        Lib = 2,

        /// <summary>
        /// Marks the widget to display the total vote count of a bot.
        /// </summary>
        /// <remarks>
        /// An example can be found here: https://top.gg/api/widget/upvotes/270904126974590976.svg
        /// </remarks>
        Upvotes = 3,

        /// <summary>
        /// Marks the widget to display the owner of a bot.
        /// </summary>
        /// <remarks>
        /// An example can be found here: https://top.gg/api/widget/owner/270904126974590976.svg
        /// </remarks>
        Owner = 4
    }
}
