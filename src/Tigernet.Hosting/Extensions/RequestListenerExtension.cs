using System.Net;
using System.Text;
using System.Text.Json;
using Tigernet.Hosting.Enums;

namespace Tigernet.Hosting.Extensions;

/// <summary>
/// Extension class for HttpListenerRequest that provides methods for retrieving data from different parts of the request.
/// </summary>
internal static class RequestListenerExtension
{
    internal static object? GetInjectingRequestData(
        this HttpListenerRequest request, 
        Type returnType, Transfer transfer,
        string selector = null!)
    {
        return transfer switch
        {
            Transfer.FromBody => request.GetDataFromRequestBody(returnType),
            Transfer.FromHeader => request.GetDataFromRequestHeader(selector, returnType),
            Transfer.FromRoute => request.GetDataFromRequestRoute(selector, returnType),
            Transfer.FromQuery => request.GetDataFromRequestQuery(returnType),
            _ => default
        };
    }
    
    internal static T? GetInjectingRequestData<T>(
        this HttpListenerRequest request, Transfer transfer,
        string selector = null!)
    {
        return transfer switch
        {
            Transfer.FromBody => request.GetDataFromRequestBody<T>(),
            Transfer.FromHeader => request.GetDataFromRequestHeader<T>(selector),
            Transfer.FromRoute => request.GetDataFromRequestRoute<T>(selector),
            Transfer.FromQuery => request.GetDataFromRequestQuery<T>(),
            _ => default
        };
    }
    
    #region Private methods

    #region Map from request body

    /// <summary>
    /// Retrieves data from the request body and returns it as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to return the data as.</typeparam>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <returns>The data extracted from the request body as the specified type.</returns>
    private static T? GetDataFromRequestBody<T>(this HttpListenerRequest request)
    {
        using Stream bodyStream = request.InputStream;
        // Determine the encoding and content type of the request
        Encoding encoding = request.ContentEncoding;

        // Read the data from the request body
        using StreamReader reader = new StreamReader(bodyStream, encoding);
        string requestBody = reader.ReadToEnd();
        
        if (requestBody != String.Empty) 
            return JsonSerializer.Deserialize<T>(requestBody);

        throw new NullReferenceException("In request body value is not given. ");
    }
    
    /// <summary>
    /// Retrieves data from the request body and returns it as the specified type.
    /// </summary>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="returnType">Returned type of the data.</param>
    /// <returns>The data extracted from the request body as the specified type.</returns>
    private static object? GetDataFromRequestBody(this HttpListenerRequest request, Type returnType)
    {
        using Stream bodyStream = request.InputStream;
        // Determine the encoding and content type of the request
        Encoding encoding = request.ContentEncoding;

        // Read the data from the request body
        using StreamReader reader = new StreamReader(bodyStream, encoding);
        string requestBody = reader.ReadToEnd();
                
        if (requestBody != String.Empty) 
            return JsonSerializer.Deserialize(requestBody, returnType);
        
        throw new NullReferenceException("In request body value is not given. ");
    }

    #endregion

    #region Map from request header
    
    /// <summary>
    /// Retrieves data from the specified header in the request and returns it as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to return the data as.</typeparam>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="key">The key of the header to extract data from.</param>
    /// <returns>The data extracted from the specified header as the specified type.</returns>
    private static T? GetDataFromRequestHeader<T>(
        this HttpListenerRequest request,
        string key)
    {
        var header = request.Headers.Get(key);
        if (header != null)
            return JsonSerializer.Deserialize<T>(header);
        
        throw new NullReferenceException($"In request header with given key '{key}' is not value is not detected");
    }

    /// <summary>
    /// Retrieves data from the specified header in the request and returns it as the specified type.
    /// </summary>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="key">The key of the header to extract data from.</param>
    /// <param name="returnType">return type of the request data</param>
    /// <returns>The data extracted from the specified header as the specified type.</returns>
    private static object? GetDataFromRequestHeader(
        this HttpListenerRequest request,
        string key, Type returnType)
    {
        string? header = request.Headers.Get(key);
        return JsonSerializer.Deserialize(header ?? "", returnType);
    }
    
    #endregion

    #region Map from request route
    
    /// <summary>
    /// Retrieves data from the specified route in the request and returns it as the specified type.
    /// </summary>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="key">The key of the route to extract data from.</param>
    /// <returns>The data extracted from the specified route as the specified type.</returns>
    private static T? GetDataFromRequestRoute<T>(
        this HttpListenerRequest request,
        string key)
    {
        var routeValue = request.Url?.Segments[^1];
        return JsonSerializer.Deserialize<T>(routeValue ?? "");
    }

    /// <summary>
    /// Retrieves data from the specified route in the request and returns it as the specified type.
    /// </summary>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="key">The key of the route to extract data from.</param>
    /// <param name="returnType">return type of the request data value</param>
    /// <returns>The data extracted from the specified route as the specified type.</returns>
    private static object? GetDataFromRequestRoute(
        this HttpListenerRequest request,
        string key, Type returnType)
    {
        var routeValue = request.Url?.Segments[^1];
        return JsonSerializer.Deserialize(routeValue ?? "", returnType);
    }

    #endregion

    #region Map from request query

    /// <summary>
    /// Retrieves data from the request query and returns it as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to return the data as.</typeparam>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <returns>The data extracted from the request query as the specified type.</returns>
    private static T? GetDataFromRequestQuery<T>(
        this HttpListenerRequest request)
    {
        var queryValue = request.QueryString.ToString();
        return JsonSerializer.Deserialize<T>(queryValue ?? "");
    }

    /// <summary>
    /// Retrieves data from the request query and returns it as the specified type.
    /// </summary>
    /// <param name="request">The HttpListenerRequest to extract data from.</param>
    /// <param name="returnType">returned type of the request coming data</param>
    /// <returns>The data extracted from the request query as the specified type.</returns>
    private static object? GetDataFromRequestQuery(
        this HttpListenerRequest request,
        Type returnType)
    {
        var queryValue = request.QueryString.ToString();
        return JsonSerializer.Deserialize(queryValue ?? "", returnType);
    }

    #endregion

    #endregion
}

