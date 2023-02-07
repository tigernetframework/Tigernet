using System.Net;
using System.Text;
using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes;
using Tigernet.Hosting.Exceptions;

namespace Tigernet.Hosting
{
    /// <summary>
    /// The TigernetHostBuilder is a class used to create and host a web server with a given prefix.
    /// It utilizes the HttpListener class from the .NET framework to listen for incoming requests and routes them to their respective handlers.
    /// The class provides a MapRoute method that maps a route URL to its corresponding request handler, which is a function that takes in an HttpListenerContext and returns a task.
    /// It also provides a MapRester method that maps a route URL to a REST API endpoint using the ResterBase class.
    /// The ResterBase class is a base class for creating REST APIs, and it uses custom attributes to identify the endpoint URLs.
    /// The Start method is used to start the web server and listen for incoming requests.
    /// </summary>
    public class TigernetHostBuilder
    {
        /// <summary>
        /// A readonly field for storing the prefix for the HTTP listener.
        /// </summary>
        private readonly string _prefix;

        /// <summary>
        /// An instance of HttpListener to listen for incoming requests.
        /// </summary>
        private readonly HttpListener _listener = new HttpListener();

        /// <summary>
        /// A dictionary to store the routes and their associated handlers functions.
        /// </summary>
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _routes = new Dictionary<string, Func<HttpListenerContext, Task>>();

        /// <summary>
        /// Constructor for TigernetHostBuilder class. It takes in a string prefix and sets it as the prefix for the HttpListener.
        /// </summary>
        /// <param name="prefix">The prefix for the HttpListener</param>
        public TigernetHostBuilder(string prefix)
        {
            _prefix = prefix;
            _listener.Prefixes.Add(prefix);
        }

        /// <summary>
        /// Method used to map a route to a specific handler function.
        /// </summary>
        /// <param name="route">The route to be mapped</param>
        /// <param name="handler">The handler function to be associated with the route</param>
        public void MapRoute(string route, Func<HttpListenerContext, Task> handler)
        {
            // check for exist of route
            if (_routes.ContainsKey(route))
                throw new RouteDublicatedException();
            
            _routes.Add(route, handler);
        }

        /// <summary>
        /// Start method starts the HTTP listener and listens for incoming requests.
        /// It writes a message in the console indicating the server is running on the specified prefix.
        /// The method uses an infinite loop to keep listening for incoming requests and handle them using the HandleRequest method.
        /// </summary>
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

        /// <summary>
        /// HandleRequest method handles the incoming request by looking for a matching route in the routes dictionary.
        /// If a matching route is found, it invokes the associated handler.
        /// If there is no matching route, it sets the response status code to "Not Found" and closes the response.
        /// </summary>
        /// <param name="context">The HttpListenerContext representing the incoming request and response</param>
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

        /// <summary>
        /// Maps the REST API endpoint for the given route and ResterBase implementation.
        /// The methods decorated with the GetterAttribute are extracted and mapped to their corresponding route URL
        /// The response is returned in JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the ResterBase implementations</typeparam>
        /// <param name="route">The base route URL for the REST API endpoints</param>
        public void MapRester<T>(string route = null) where T : ResterBase, new()
        {
            var rester = new T();
            var type = rester.GetType();
            var typeName = type.Name;
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(GetterAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as GetterAttribute;

                    // if route is null, use the route from the class name
                    if (string.IsNullOrEmpty(route))
                    {
                        route = Path.Combine("/", typeName.Split(new[] { "Rester" }, 
                            StringSplitOptions.None).FirstOrDefault());
                    }
                    
                    var routeUrl = (route + attribute.route).ToLower();
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
