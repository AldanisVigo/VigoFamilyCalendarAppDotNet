using System.Text.Json;
using System.Text.Json.Serialization;
namespace FamilyCalendarDotNet;

public class JsonDateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var d = reader.GetDateTime();
        return d;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        if (value.Kind == DateTimeKind.Unspecified)
        {
            value = new DateTime(value.Ticks, DateTimeKind.Utc);
        }
        writer.WriteStringValue(value);
    }
}