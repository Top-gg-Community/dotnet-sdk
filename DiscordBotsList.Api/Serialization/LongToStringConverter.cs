using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBotsList.Api.Internal
{
    /// <summary>
    /// Converts API responses from strings to longs and vice versa.
    /// </summary>
    internal class ULongToStringConverter : JsonConverter<ulong>
    {
        public override ulong Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if(ulong.TryParse(stringValue, out var value))
            {
                return value;
            }
            throw new InvalidOperationException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            ulong value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
