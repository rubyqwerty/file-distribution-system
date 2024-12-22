
public class VirtualNodeManager
{
    public VirtualNodeManager(storage.IStorage storage, IHashManager hashManager, ILogger<VirtualNodeManager> logger)
    {
        _storageProvider = storage;
        _hashManager = hashManager;
        _logger = logger;
    }

    public async void CreateVirtualNodes(models.Server server)
    {
        for (int i = 0; i < server.Priority; ++i)
        {
            var hashedData = server.Address
                    + server.Priority.ToString()
                    + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            var hash = await _hashManager.GetHash(new hash_service.HashParams()
            {
                Data = hashedData,
                NumberIteration = 2,
                Algorithm = hash_service.Algorithms.SHA256,
                OutputFormat = hash_service.Format.HEX
            });

            _logger.LogInformation($"Триггер создание виртуальных узлов: id {server.Id}, hash {hash}");

            await _storageProvider.AddVirtualNode(new models.VirtualNode()
            {
                Hash = hash,
                IdServer = server.Id
            });
        }
    }

    public void RecreateVirtualNodes(models.Server server)
    {
        DeleteVirtualNodes(server);
        CreateVirtualNodes(server);
    }


    public void DeleteVirtualNodes(models.Server server)
    {
        _logger.LogInformation($"Триггер удаление виртуальных узлов от сервера с id {server.Id}");
        _storageProvider.DeleteVirtualNodes(server.Id);
    }

    private readonly ILogger<VirtualNodeManager> _logger;
    private readonly storage.IStorage _storageProvider;
    private readonly IHashManager _hashManager;
}