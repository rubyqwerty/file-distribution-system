using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<DistributionServiceHandler>();
        services.AddHostedService<DistributionsThriftServer>();
    })
    .Build();

host.Run();