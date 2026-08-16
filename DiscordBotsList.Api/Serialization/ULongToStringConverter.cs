using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    /// <summary>
    ///     Converts API responses from strings or numbers to ulongs and vice versa.
    ///     Top.gg often represents snowflake IDs as strings, so a plain ulong binding
    ///     would fail without this converter.
    /// </summary>
    internal class ULongToStringConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String when ulong.TryParse(reader.GetString(), out var stringValue):
                    return stringValue;

                case JsonTokenType.Number when reader.TryGetUInt64(out var numberValue):
                    return numberValue;

                case JsonTokenType.Null:
                    return default;

                default:
                    throw new JsonException($"Cannot convert {reader.TokenType} to ulong.");
            }
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
