using DataAccessLayer.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi;

public class JsonNumberEnumConverter : JsonConverter<ServerType> {
    public override ServerType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Number) {
            int value = reader.GetInt32();
            if (Enum.IsDefined(typeof(ServerType), value)) {
                return (ServerType)value;
            }
        }

        throw new JsonException($"Unable to convert {reader.GetString()} to ServerType");
    }

    public override void Write(Utf8JsonWriter writer, ServerType value, JsonSerializerOptions options) {
        writer.WriteNumberValue((int)value);
    }
}