
public class VirtualNodeManager
{
    public VirtualNodeManager(storage.IStorage storage, IHashManager hashManager)
    {
        _storageProvider = storage;
        _hashManager = hashManager;
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
        _storageProvider.DeleteVirtualNodes(server.Id);
    }

    private readonly storage.IStorage _storageProvider;

    private readonly IHashManager _hashManager;
}