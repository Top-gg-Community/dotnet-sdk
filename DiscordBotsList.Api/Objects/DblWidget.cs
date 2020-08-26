namespace DiscordBotsList.Api.Objects
{
    internal class DblWidget : IDblWidget
    {
        public ulong BotId { get; }
        public WidgetImageFormat Format { get; }
        public WidgetSize Size { get; }
        public string ImageUrl { get; }
    }
}
