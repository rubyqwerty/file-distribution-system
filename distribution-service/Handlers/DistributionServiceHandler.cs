using Microsoft.Extensions.Logging;

public class DistributionServiceHandler : distribution_service.DistributionService.IAsync
{
    public DistributionServiceHandler(ILogger<DistributionServiceHandler> logger, storage.IStorage storage)
    {
        _logger = logger;
        _storage = storage;
        _logger.LogInformation("Создан обработчик запросов службы репликаций");
    }

    public async Task MakeReplications(
        List<distribution_service.Chunk> requestChunks,
        CancellationToken cancellationToken)
    {
        try
        {

            _logger.LogInformation($"Получен запрос на создание репликаций для файла с id: {requestChunks.FirstOrDefault().IdMetadata}");

            var serverNodes = await _storage.GetVirtualNodes();

            List<ICircleNode> chunks = [];

            await Task.Run(() =>
            {
                foreach (var chunk in requestChunks)
                {
                    chunks.Add(new CircleChunkNode()
                    {
                        hash = chunk.ChunkHash,
                        generalFile = chunk.IdMetadata
                    });
                }
            });

            List<ICircleNode> nodes = [];

            await Task.Run(() =>
          {
              foreach (var node in serverNodes)
              {
                  nodes.Add(
                        new CircleServerNode()
                        {
                            hash = node.Hash,
                            generalServer = node.IdServer
                        }
                    );
              }
          });

            var placer = new PlaceFounder(nodes, chunks, 3);

            placer.ComputePlacement();

            var placements = placer.Placement;

            List<models.Replication> replications = [];

            foreach (var placement in placements)
            {
                var replication = new models.Replication()
                {
                    IdServer = ((CircleServerNode)placement.virtualServerNode).generalServer,
                    HashChunk = placement.chunk.hash,
                    IdMetadata = ((CircleChunkNode)placement.chunk).generalFile,
                };

                var server = await _storage.GetServer(replication.IdServer);

                replications.Add(replication);

                _logger.LogInformation($"Чанк {replication.HashChunk} --> Сервер {server.Address}");
            }

            await _storage.AddReplications(replications);

            _logger.LogInformation("Репликации были успешно созданы");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(message: $"Исключение: {ex}");
            throw;
        }
    }

    private readonly ILogger<DistributionServiceHandler> _logger;

    private readonly storage.IStorage _storage;
}