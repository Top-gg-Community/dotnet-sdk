namespace DiscordBotsList.Api.Objects
{
    public interface IDblWidget
    {
        ulong BotId { get; }

        WidgetImageFormat Format { get; }

        WidgetSize Size { get; }

        string ImageUrl { get; }
    }
}
