using System.Text;
using Tigernet.Hosting;

TigernetHostBuilder host = new TigernetHostBuilder();

// add route to the host
host.MapRoute("/", async context =>
{
    var response = context.Response;
    response.ContentType = "text/html";
    var content = Encoding.UTF8.GetBytes("<h1>Welcome to Tigernet Framework</h1>");
    response.ContentLength64 = content.Length;
    using (var output = response.OutputStream)
    {
        await output.WriteAsync(content, 0, content.Length);
    }
});

host.MapRoute("/about/{id}", async context =>
{
    var response = context.Response;
    response.ContentType = "text/html";
    var content = Encoding.UTF8.GetBytes("<h1>About Tigernet Framework</h1>");
    response.ContentLength64 = content.Length;
    using (var output = response.OutputStream)
    {
        await output.WriteAsync(content, 0, content.Length);
    }
});

host.MapRoute("/contact", async context =>
{
    var response = context.Response;
    response.ContentType = "text/html";
    var content = Encoding.UTF8.GetBytes("<h1>Contact Tigernet Framework</h1>");
    response.ContentLength64 = content.Length;
    using (var output = response.OutputStream)
    {
        await output.WriteAsync(content, 0, content.Length);
    }
});

await host.Start();