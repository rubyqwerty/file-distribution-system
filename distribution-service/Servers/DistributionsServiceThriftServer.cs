
using distribution_service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport.Server;

public class DistributionsThriftServer : IHostedService
{
    public DistributionsThriftServer(ILogger<DistributionServiceHandler> logger, DistributionServiceHandler handler)
    {
        _logger = logger;

        var processor = new DistributionService.AsyncProcessor(handler);

        _serverTransport = new TServerSocketTransport(9091, new TConfiguration(), 1);

        _server = new TSimpleAsyncServer(
             processor,
             _serverTransport,
             new TBinaryProtocol.Factory(),
             new TBinaryProtocol.Factory(),
             new LoggerFactory()
         );
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Служба репликаций запушена");
        await _server.ServeAsync(cancellationToken);

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Служба репликаций остановлена");
        _server.Stop();
        _serverTransport.Close();

        await Task.CompletedTask;
    }

    private readonly TServer _server;

    private readonly TServerSocketTransport _serverTransport;

    private readonly ILogger _logger;

}