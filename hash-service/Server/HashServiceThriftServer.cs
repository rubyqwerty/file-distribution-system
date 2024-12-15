
using hash_service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport.Server;

public class HashThriftServer : IHostedService
{
    public HashThriftServer(ILogger<HashServiceHandler> logger, HashServiceHandler handler)
    {
        _logger = logger;

        var processor = new HashService.AsyncProcessor(handler);

        _serverTransport = new TServerSocketTransport(9090, new TConfiguration(), 1);

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
        _logger.LogInformation("Служба хеширования запушена");
        await _server.ServeAsync(cancellationToken);

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Служба хеширования остановлена");
        _server.Stop();
        _serverTransport.Close();

        await Task.CompletedTask;
    }

    private readonly TServer _server;

    private readonly TServerSocketTransport _serverTransport;

    private readonly ILogger _logger;

}