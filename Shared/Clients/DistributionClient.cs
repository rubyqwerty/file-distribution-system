
using Thrift.Protocol;
using Thrift.Transport.Client;

public interface IDistributionManager
{
    public Task MakeReplication(List<distribution_service.Chunk> chunks);
}

public class DistributionManager : IDistributionManager
{
    public async Task MakeReplication(List<distribution_service.Chunk> chunks)
    {
        var transport = new TSocketTransport("localhost", 9091, new Thrift.TConfiguration());
        var protocol = new TBinaryProtocol(transport);
        var client = new distribution_service.DistributionService.Client(protocol);

        await client.MakeReplications(chunks);
    }
}