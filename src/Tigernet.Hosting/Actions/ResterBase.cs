using System.Text.Json;

namespace Tigernet.Hosting.Actions
{
    public abstract class ResterBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public object Ok<T>(T data)
        {
            return JsonSerializer.Serialize(data, _jsonSerializerOptions);
        }
    }
}