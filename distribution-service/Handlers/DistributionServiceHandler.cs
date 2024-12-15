using Microsoft.Extensions.Logging;
using nodes;

public class DistributionServiceHandler : distribution_service.DistributionService.IAsync
{
    public DistributionServiceHandler(ILogger<DistributionServiceHandler> logger, IServerNodesProvider serverNodesProvider, IStorage storage)
    {
        _logger = logger;
        _serverNodesHandler = serverNodesProvider;
        _storage = storage;
        _logger.LogInformation("Создан обработчик запросов службы репликаций");
    }

    public async Task<List<distribution_service.Replication>> MakeReplications(
        List<distribution_service.Chunk> requestChunks,
        CancellationToken cancellationToken)
    {
        try
        {
            var serverNodes = _serverNodesHandler.GetServerNodes();

            List<ICircleNode> chunks = [];

            await Task.Run(() =>
            {
                foreach (var chunk in requestChunks)
                {
                    chunks.Add(new CircleChunkNode()
                    {
                        hash = chunk.ChunkHash
                    });
                }
            });

            var placer = new PlaceFounder(serverNodes, chunks, 3);

            placer.ComputePlacement();

            var placements = placer.Placement;

            // List<distribution_service.Replication> replicationsResponse = [];

            // foreach (var placement in placements)
            // {
            //     new distribution_service.Replication()
            //     {
            //         Chunk = new distribution_service.Chunk()
            //         {
            //             ChunkHash = placement.chunk.hash,
            //             IdMetadata = requestChunks[0].IdMetadata
            //         },
            //         IdServer = placement.virtualServerNode.
            //     };
            // }

        }
        catch (Exception ex)
        {
            _logger.LogWarning(message: $"Исключение: {ex}");
            throw;
        }
    }

    public async Task<List<distribution_service.Replication>> GetReplications(int idMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var replications = await _storage.GetReplications(idMetadata);

            List<distribution_service.Replication> replicationsResponse = [];

            await Task.Run(() =>
            {
                foreach (var replication in replications)
                {
                    replicationsResponse.Add(new distribution_service.Replication()
                    {
                        Chunk = new()
                        {
                            IdMetadata = replication.Chunk.IdMetadata,
                            ChunkHash = replication.Chunk.Hash
                        },
                        IdServer = replication.IdServer
                    });
                }
            });

            return replicationsResponse;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(message: $"Исключение: {ex}");
            throw;
        }
    }

    private readonly ILogger<DistributionServiceHandler> _logger;

    private readonly IServerNodesProvider _serverNodesHandler;

    private readonly IStorage _storage;
}