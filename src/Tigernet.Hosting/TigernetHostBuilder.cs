using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Tigernet.Hosting.Attributes.HttpMethods;
using Tigernet.Hosting.Attributes.RequestContents;
using Tigernet.Hosting.Attributes.Resters;
using Tigernet.Hosting.Exceptions;
using Tigernet.Hosting.Extensions;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tigernet.Hosting; 

#pragma warning disable
/// <summary>
/// The TigernetHostBuilder is a class used to create and host a web server with a given prefix.
/// It utilizes the HttpListener class from the .NET framework to listen for incoming requests and routes them to their respective handlers.
/// The class provides a MapRoute method that maps a route URL to its corresponding request handler, which is a function that takes in an HttpListenerContext and returns a task.
/// It also provides a MapRester method that maps a route URL to a REST API endpoint using the ResterBase class.
/// The ResterBase class is a base class for creating REST APIs, and it uses custom attributes to identify the endpoint URLs.
/// The Start method is used to start the web server and listen for incoming requests.
/// </summary>
public partial class TigernetHostBuilder
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
    /// A list of middleware functions.
    /// </summary>
    internal readonly List<Func<HttpListenerContext, Task>> _middlewares = new List<Func<HttpListenerContext, Task>>();

    /// <summary>
    /// Constructor for TigernetHostBuilder class. It takes in a string prefix and sets it as the prefix for the HttpListener.
    /// </summary>
    /// <param name="prefix">The prefix for the HttpListener</param>
    public TigernetHostBuilder(string prefix)
    {
        _prefix = prefix;
        _listener.Prefixes.Add(prefix);
        _services = new Dictionary<Type, Type>();
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
            throw new RouteDuplicatedException(route);

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
            await HandleRequestAsync(context);
        }
    }

    /// <summary>
    /// HandleRequest method handles the incoming request by looking for a matching route in the routes dictionary.
    /// If a matching route is found, it invokes the associated handler.
    /// If there is no matching route, it sets the response status code to "Not Found" and closes the response.
    /// </summary>
    /// <param name="context">The HttpListenerContext representing the incoming request and response</param>
    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        // get the route from the request URL
        // apply _middlewares to the request and response here
        var request = context.Request;
        var response = context.Response;

        // check middleware is exist
        if (_middlewares.Any())
        {
            foreach (var next in _middlewares)
            {
                // check route is exist
                if (!_routes.ContainsKey(request.Url.AbsolutePath))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Close();

                    return;
                }
                else
                {
                    await next(context);
                }
            }
        }

        // if middleware is not exist
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
    /// Using middleware
    /// </summary>
    /// <param name="middleware"></param>
    /// <returns></returns>
    public TigernetHostBuilder UseAsync(Func<HttpListenerContext, Task> middleware)
    {
        _middlewares.Add(middleware);

        return this;
    }

    public void MapResters()
    {
        // get the assembly that is using this library
        var assembly = Assembly.GetCallingAssembly();

        // get all types in the assembly
        var types = assembly.GetTypes();

        // filter for types that have the ApiRester attribute
        var resterTypes = types.Where(t => t.GetCustomAttribute<ApiResterAttribute>() != null);

        foreach (var resterType in resterTypes)
        {
            var methods = resterType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<HttpMethodAttribute>() != null);

            var typeName = resterType.Name;

            foreach (var method in methods)
            {
                var getterAttr = method.GetCustomAttribute<GetterAttribute>();
                var posterAttr = method.GetCustomAttribute<PosterAttribute>();
                var patcherAttr = method.GetCustomAttribute<PatcherAttribute>();
                var putterAttr = method.GetCustomAttribute<PutterAttribute>();

                HttpMethodAttribute? endpointAttr = GetValidatedMethodAttributes(new HttpMethodAttribute[]
                {
                    getterAttr, posterAttr, patcherAttr, putterAttr
                });

                var route = Path.Combine("/", typeName.Split(new[] { "Rester" },
                        StringSplitOptions.None).FirstOrDefault());

                var routeUrl = (route + endpointAttr.route).ToLower();

                var handler = CreateHandlerFunc(resterType, method, endpointAttr);

                MapRoute(routeUrl, handler);
            }
        }
    }
    
    /// <summary>
    /// Retrieves a validated HttpMethodAttribute from an array of HttpMethodAttributes.
    /// </summary>
    /// <param name="httpMethodAttributes">An array of HttpMethodAttributes.</param>
    /// <returns>The validated HttpMethodAttribute.</returns>
    /// <exception cref="ArgumentException">Thrown when the array of HttpMethodAttributes contains invalid usage.</exception>
    HttpMethodAttribute? GetValidatedMethodAttributes(HttpMethodAttribute?[] httpMethodAttributes)
    {
        if (httpMethodAttributes.Count(a => a != null) != 1)
            throw new ArgumentException("Invalid usage http method attributes");

        return httpMethodAttributes.FirstOrDefault(a => a != null);
    }

    /// <summary>
    /// Creates a handler function for processing HTTP requests.
    /// </summary>
    /// <param name="resterType">The type of the RESTful service.</param>
    /// <param name="method">The method to invoke on the RESTful service.</param>
    /// <param name="attribute">The HTTP method attribute associated with the method.</param>
    /// <returns>A delegate representing the handler function.</returns>
    private Func<HttpListenerContext, Task> CreateHandlerFunc(Type resterType, MethodInfo method, HttpMethodAttribute attribute)
    {
        return async context =>
        {
            if (context.Request.HttpMethod != attribute.HttpMethodName)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.Close();
            }
            else
            {
                object rester;
                var constructor = resterType.GetConstructors().FirstOrDefault();

                if (constructor != null)
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new object[parameters.Length];

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameterType = parameters[i].ParameterType;
                        var service = GetService(parameterType);
                        if (service != null)
                        {
                            parameterInstances[i] = service;
                        }
                        else
                        {
                            throw new Exception(
                                $"Unable to resolve service of type {parameterType} for constructor of {resterType}.");
                        }
                    }

                    rester = constructor.Invoke(parameterInstances);
                }
                else
                {
                    rester = Activator.CreateInstance(resterType);
                }

                var args = await GetArguments(method, context);
                var result = method.Invoke(rester, args);

                if (result is Task task)
                {
                    await task;
                }

                var response = context.Response;
                var content = Encoding.UTF8.GetBytes(result.ToString());
                response.ContentLength64 = content.Length;
                using (var output = response.OutputStream)
                {
                    await output.WriteAsync(content, 0, content.Length);
                }
            }
        };
    }

    /// <summary>
    /// Retrieves the arguments to be passed to the RESTful service method.
    /// </summary>
    /// <param name="method">The method to invoke on the RESTful service.</param>
    /// <param name="context">The HTTP listener context.</param>
    /// <returns>An array of objects representing the arguments to be passed to the method.</returns>
    private async ValueTask<object[]> GetArguments(MethodInfo method, HttpListenerContext context)
    {
        var parameters = method.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var parameterAttributes = 
                                    parameters[i].GetCustomAttributes<RequestContentAttribute>() 
                                    ?? new List<RequestContentAttribute>();
            if (parameterAttributes.Any())
            {
                if (parameterAttributes.Count() > 1)
                    throw new ArgumentException($"In {method.Name} argument have multiple http request content. In parameter {parameters[i].Name}");
                
                var parameterType = parameters[i].ParameterType;
                args[i] = context.Request.GetInjectingRequestData(parameterType,
                    parameterAttributes!.First()!.DataMapSource);
            }
            else
            {
                args[i] = null;
            }

           // var content = await GetRequestContentAsync(context.Request, parameterType);
           // if (content != default)
           //     args[i] = content;
        }

        return args;
    }

    /// <summary>
    /// Retrieves the content of the HTTP request body and deserializes it to the expected type.
    /// </summary>
    /// <param name="request">The HTTP listener request.</param>
    /// <param name="expectedType">The expected type of the deserialized object.</param>
    /// <returns>The deserialized object, or null if the request has no content or deserialization fails.</returns>
    private async ValueTask<object?> GetRequestContentAsync(HttpListenerRequest request, Type expectedType)
    {
        if (request.ContentLength64 == 0)
            return null;

        using var reader = new StreamReader(request.InputStream);
        var content = await reader.ReadToEndAsync();
        var result = default(object);
        try
        {
            result = JsonConvert.DeserializeObject(content, expectedType);
        }
        catch (Exception exception)
        {
            // TODO: Log exception
        }

        return result;
    }
}