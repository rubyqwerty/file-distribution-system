using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<HashServiceHandler>();
        services.AddHostedService<HashThriftServer>();
    })
    .Build();

host.Run();