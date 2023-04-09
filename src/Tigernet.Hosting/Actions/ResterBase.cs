using System.Text.Json;

namespace Tigernet.Hosting.Actions
{
    public abstract class ResterBase
    {
        public object Ok(object data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}