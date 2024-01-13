using System.Text.Json;
using System.Text.Json.Serialization;

namespace almondcove.Modules
{
    public static class JsonSerializationHelper
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };


        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
    }
}
