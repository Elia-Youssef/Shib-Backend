using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StringValueConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Convert any type of value to string
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble().ToString();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }
        else if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else if (reader.TokenType == JsonTokenType.True)
        {
            return "true";
        }
        else if (reader.TokenType == JsonTokenType.False)
        {
            return "false";
        }

        throw new JsonException("Unsupported token type for string conversion");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

