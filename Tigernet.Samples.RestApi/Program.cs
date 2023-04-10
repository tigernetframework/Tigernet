using Tigernet.Hosting;
using Tigernet.Samples.RestApi.Clevers;
using Tigernet.Samples.RestApi.Clevers.Interfaces;

var builder = new TigernetHostBuilder();

builder.AddService<IUserClever, UserClever>();

builder.MapResters();

//builder.UseAsync(async (context) =>
//{
//    Console.WriteLine("Middleware 1");
//    await Task.CompletedTask;
//});

//builder.UseAsync(async (context) =>
//{
//    Console.WriteLine("Middleware 2");
//    await Task.CompletedTask;
//});

await builder.Start();

