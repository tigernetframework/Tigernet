namespace Tigernet.Hosting.Http
{
    public class HttpContext
    {
        public HttpContext(HttpRequestMessage request, HttpResponseMessage response)
        {
            this.Request = request;
            this.Response = response;
        }
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}
