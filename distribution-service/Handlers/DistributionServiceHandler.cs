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
       int IdMetadata,
        CancellationToken cancellationToken)
    {
        try
        {

            var chunks = await _storage.GetChunkByMetadataId(IdMetadata);

            _logger.LogInformation($"Получен запрос на создание репликаций для файла с id: {IdMetadata}");

            var serverNodes = await _storage.GetVirtualNodes();

            List<ICircleNode> chunkNodes = [];

            await Task.Run(() =>
            {
                foreach (var chunk in chunks)
                {
                    chunkNodes.Add(new CircleChunkNode()
                    {
                        hash = chunk.Hash,
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
                        }
                    );
              }
          });

            var placer = new PlaceFounder(nodes, chunkNodes, 1);

            placer.ComputePlacement();

            var placements = placer.Placement;

            List<models.Replication> replications = [];

            foreach (var placement in placements)
            {
                var replication = new models.Replication()
                {
                    IdServer = _storage.GetServerByVirtualNodeHash(placement.virtualServerNode.hash).Result.Id,
                    HashChunk = placement.chunk.hash,
                    IdMetadata = _storage.GetMetadataByChunkHash(placement.chunk.hash).Result.Id,
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