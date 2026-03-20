using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Topgg.Sdk.Webhooks.Serialization;

/// <summary>Converts API responses from strings to longs and vice versa.</summary>
internal class ULongToStringConverter : JsonConverter<ulong>
{
    public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && ulong.TryParse(reader.GetString(), out var value))
        {
            return value;
        }

        throw new InvalidOperationException();
    }

    public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}