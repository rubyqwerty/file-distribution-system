

using Thrift.Protocol;
using Thrift.Transport.Client;

public interface IDistributionManager
{
    public Task MakeReplication(int idMetadata);
}

public class DistributionManager : IDistributionManager
{
    public async Task MakeReplication(int idMetadata)
    {
        var host = Environment.GetEnvironmentVariable("DISTRIBUTION_HOST") ?? "127.0.0.1";
        var transport = new TSocketTransport(host, 9091, new Thrift.TConfiguration());
        var protocol = new TBinaryProtocol(transport);
        var client = new distribution_service.DistributionService.Client(protocol);

        await client.MakeReplications(idMetadata);

        transport.Close();
    }
}