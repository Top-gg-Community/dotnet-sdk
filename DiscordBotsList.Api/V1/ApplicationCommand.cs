using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api
{
    /// <summary>
    ///     A Discord slash command definition sent to <c>PUT /projects/@me/commands</c>.
    ///     Follows <a href="https://discord.com/developers/docs/interactions/application-commands#application-command-object">Discord's application command structure</a>.
    /// </summary>
    public class ApplicationCommand
    {
        public ApplicationCommand()
        {
        }

        public ApplicationCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        [JsonPropertyName("type")]
        public int? Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("name_localizations")]
        public Dictionary<string, string> NameLocalizations { get; set; }

        [JsonPropertyName("description_localizations")]
        public Dictionary<string, string> DescriptionLocalizations { get; set; }

        [JsonPropertyName("options")]
        public List<ApplicationCommandOption> Options { get; set; }

        [JsonPropertyName("nsfw")]
        public bool? Nsfw { get; set; }
    }

    /// <summary>An option of an application command.</summary>
    public class ApplicationCommandOption
    {
        [JsonPropertyName("type")]
        public int? Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("name_localizations")]
        public Dictionary<string, string> NameLocalizations { get; set; }

        [JsonPropertyName("description_localizations")]
        public Dictionary<string, string> DescriptionLocalizations { get; set; }

        [JsonPropertyName("required")]
        public bool? Required { get; set; }

        [JsonPropertyName("choices")]
        public List<ApplicationCommandOptionChoice> Choices { get; set; }

        [JsonPropertyName("options")]
        public List<ApplicationCommandOption> Options { get; set; }

        [JsonPropertyName("channel_types")]
        public List<int> ChannelTypes { get; set; }

        [JsonPropertyName("min_length")]
        public int? MinLength { get; set; }

        [JsonPropertyName("max_length")]
        public int? MaxLength { get; set; }

        [JsonPropertyName("min_value")]
        public JsonElement? MinValue { get; set; }

        [JsonPropertyName("max_value")]
        public JsonElement? MaxValue { get; set; }

        [JsonPropertyName("autocomplete")]
        public bool? Autocomplete { get; set; }
    }

    /// <summary>A choice of an application command option.</summary>
    public class ApplicationCommandOptionChoice
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_localizations")]
        public Dictionary<string, string> NameLocalizations { get; set; }

        [JsonPropertyName("value")]
        public JsonElement? Value { get; set; }
    }
}
