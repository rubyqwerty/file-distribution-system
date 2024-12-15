
public interface IStorage
{
    internal Task<List<models.Replication>> GetReplications(int idMetadata);
    internal Task AddReplication(models.Replication replication);
}