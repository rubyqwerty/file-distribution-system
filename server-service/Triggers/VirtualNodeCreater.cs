
public class VirtualNodeManager
{
    public VirtualNodeManager(IStorage storage, IHashManager hashManager)
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
                Algorithm = hash_service.Algorithms.SHA256
            });

            await _storageProvider.AddVirtualNode(new models.VirtualNode()
            {
                Hash = hash,
                IdServer = server.Id
            });
        }
    }

    public void DeleteVirtualNodes(int idServer)
    {

    }

    private readonly IStorage _storageProvider;

    private readonly IHashManager _hashManager;
}