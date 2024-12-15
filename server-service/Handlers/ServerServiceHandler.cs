using server_service;
using Microsoft.Extensions.Logging;
using Thrift.Protocol;
using Thrift.Server;


public class ServerServiceHandler : ServerService.IAsync
{
    public ServerServiceHandler(ILogger<ServerServiceHandler> logger, IStorage storage)
    {
        _logger = logger;
        _storageProvider = storage;
        _logger.LogInformation("Создан обработчик запросов хеш");
    }

    public async Task<Server> GetServer(int idServer, CancellationToken cancellationToken)
    {
        var server = await _storageProvider.GetServer(idServer);

        if (server == null)
        {
            var errorMessage = $"Пришел запрос на получение сервера с id {idServer}, но такого нет";
            _logger.LogWarning(errorMessage);
            throw new Exception(errorMessage);
        }

        return new Server()
        {
            IdServer = server.Id,
            Address = server.Address
        };
    }

    public async Task<List<VirtualNode>> GetVirtualNodes(CancellationToken cancellationToken)
    {
        var virtualNodes = await _storageProvider.GetVirtualNodes();

        if (virtualNodes == null)
        {
            var errorMessage = $"Ошибка получения виртуальных узлов (пусто)";
            _logger.LogWarning(errorMessage);
            throw new Exception(errorMessage);
        }

        List<VirtualNode> virtualNodesResponse = [];

        await Task.Run(() =>
        {
            foreach (var node in virtualNodes)
            {
                virtualNodesResponse.Add(
                    new VirtualNode()
                    {
                        IdServer = node.IdServer,
                        Hash = node.Hash
                    }
                );
            }
        });

        return virtualNodesResponse;

    }

    private readonly ILogger<ServerServiceHandler> _logger;

    private readonly IStorage _storageProvider;

}