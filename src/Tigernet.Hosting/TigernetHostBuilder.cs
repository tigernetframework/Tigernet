using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes;

namespace Tigernet.Hosting
{
    public partial class TigernetHostBuilder
    {
        private readonly string _prefix;
        private readonly HttpListener _listener = new HttpListener();
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _routes = new Dictionary<string, Func<HttpListenerContext, Task>>();

        public TigernetHostBuilder(string prefix)
        {
            _prefix = prefix;
            _listener.Prefixes.Add(prefix);
            _services = new Dictionary<Type, Type>();
        }


        public void MapRoute(string route, Func<HttpListenerContext, Task> handler)
        {
            _routes.Add(route, handler);
        }

        public async Task Start()
        {
            _listener.Start();
            Console.WriteLine("Server is running on " + _prefix);
            while (true)
            {
                var context = await _listener.GetContextAsync();
                await HandleRequest(context);
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Func<HttpListenerContext, Task> handler;
            if (_routes.TryGetValue(request.RawUrl, out handler))
            {
                await handler(context);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
            }
        }

        public void MapRester<T>(string route) where T : ResterBase
        {
            T rester;
            var type = typeof(T);
            var constructor = type.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                rester = (T)Activator.CreateInstance(type);
            }

            else
            {
                var parameterInstances = new object[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    parameterInstances[i] = GetService(parameters[i].ParameterType);
                }

                rester = (T)constructor.Invoke(parameterInstances);
            }
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(GetterAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as GetterAttribute;

                    var routeUrl = route + attribute.Route;
                    MapRoute(routeUrl, async context =>
                    {
                        var response = context.Response;
                        response.ContentType = "application/json";
                        var result = method.Invoke(rester, null);
                        var content = Encoding.UTF8.GetBytes(result.ToString());
                        response.ContentLength64 = content.Length;
                        using (var output = response.OutputStream)
                        {
                            await output.WriteAsync(content, 0, content.Length);
                        }
                    });
                }
            }
        }
    }
}