using Tigernet.Hosting;
using Tigernet.Samples.RestApi.Resters;

var builder = new TigernetHostBuilder("http://localhost:5000/");

// add rester to the host
builder.MapRester<UsersRester>();
builder.MapRester<HomeRester>();

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

