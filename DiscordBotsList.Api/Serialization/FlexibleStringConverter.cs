using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    /// <summary>
    ///     Accepts either a JSON string or a JSON number and exposes it as a string.
    ///     Used for fields such as <c>platform</c> and <c>type</c>, which the API may
    ///     serialize as string enum names or as their underlying integer values.
    /// </summary>
    internal class FlexibleStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();

                case JsonTokenType.Number:
                case JsonTokenType.True:
                case JsonTokenType.False:
                    return Encoding.UTF8.GetString(reader.ValueSpan);

                case JsonTokenType.Null:
                    return null;

                default:
                    using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                    {
                        return doc.RootElement.GetRawText();
                    }
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
