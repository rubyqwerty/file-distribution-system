using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var routingConfigPath = Environment.GetEnvironmentVariable("ROUTING_CONFIG") ?? "Properties/routing.json";

builder.Configuration.AddJsonFile(routingConfigPath);

builder.Services.AddOcelot();

var app = builder.Build();

app.UseOcelot().Wait();

app.MapControllers();

app.Run();
