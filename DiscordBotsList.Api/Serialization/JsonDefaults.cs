using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    /// <summary>
    ///     Shared JSON serializer options used across the v1 API client and the
    ///     webhook helpers. IDs are read from strings or numbers, null properties are
    ///     omitted when writing request bodies, and numeric values may be read from
    ///     strings for maximum compatibility with the live API.
    /// </summary>
    internal static class JsonDefaults
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters =
            {
                new ULongToStringConverter(),
                new FlexibleStringConverter(),
            },
        };
    }
}
