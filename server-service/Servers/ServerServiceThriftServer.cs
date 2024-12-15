
using server_service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport.Server;

public class ServerNodeThriftServer : IHostedService
{
    public ServerNodeThriftServer(ILogger<ServerServiceHandler> logger, ServerServiceHandler handler)
    {
        _logger = logger;

        var processor = new ServerService.AsyncProcessor(handler);

        _serverTransport = new TServerSocketTransport(9093, new TConfiguration(), 1);

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
        _logger.LogInformation("Служба обработки серверов запушена");
        await _server.ServeAsync(cancellationToken);

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Служба обработки серверов остановлена");
        _server.Stop();
        _serverTransport.Close();

        await Task.CompletedTask;
    }

    private readonly TServer _server;

    private readonly TServerSocketTransport _serverTransport;

    private readonly ILogger _logger;

}