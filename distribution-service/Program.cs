using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using storage;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IStorage, Storage>();
        services.AddTransient<DistributionServiceHandler>();
        services.AddHostedService<DistributionsThriftServer>();
    })
    .Build();

host.Run();