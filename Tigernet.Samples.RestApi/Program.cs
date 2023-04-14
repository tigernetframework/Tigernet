using Tigernet.Hosting;
using Tigernet.Hosting.DataAccess.Brokers;
using Tigernet.Samples.RestApi.Brokers;

var builder = new TigernetHostBuilder("http://localhost:5000/");

// builder.AddService<IUserEntityManager, UserEntityManager>();
builder.AddDataSourceProvider<IDataSourceBroker, EfCoreDataAccessBroker>();

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