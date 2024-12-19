using hash_service;
using Thrift.Protocol;
using Thrift.Transport.Client;

public interface IHashManager
{
    public Task<string> GetHash(HashParams hashParams);
}

public class HashManager : IHashManager
{
    public async Task<string> GetHash(HashParams hashParams)
    {
        var transport = new TSocketTransport("localhost", 9090, new Thrift.TConfiguration());
        var protocol = new TBinaryProtocol(transport);
        var client = new HashService.Client(protocol);

        var hash = await client.GetHash(hashParams);

        transport.Close();

        return hash;
    }
}