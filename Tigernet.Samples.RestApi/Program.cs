using Tigernet.Hosting;
using Tigernet.Samples.RestApi.Resters;

var builder = new TigernetHostBuilder("http://localhost:5001/");

// add rester to the host
builder.MapRester<UsersRester>();
builder.MapRester<HomeRester>();

await builder.Start();

