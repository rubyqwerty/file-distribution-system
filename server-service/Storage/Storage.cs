public interface IStorage
{
    public Task<List<models.Server>> GetServers();
    public Task<models.Server> GetServer(int idServer);
    public Task<List<models.VirtualNode>> GetVirtualNodes();
    public Task AddServer(models.Server server, Action<models.Server> callback);
    public Task AddVirtualNode(models.VirtualNode virtualNode);
    public Task DeleteServer(int idServer, Action<int> callback);
}