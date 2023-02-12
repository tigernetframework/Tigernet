using Tigernet.Hosting;
using Tigernet.Samples.RestApi.Abstractions;
using Tigernet.Samples.RestApi.Clevers;
using Tigernet.Samples.RestApi.Resters;

var builder = new TigernetHostBuilder("http://localhost:5001/");

// add rester to the host
builder.AddService<IUserClever, UserClever>();

builder.MapRester<UsersRester>("/users");
builder.MapRester<HomeRester>("/");

await builder.Start();

