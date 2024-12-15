namespace netstd distribution_service

struct Chunk
{
    1: i32 idMetadata,
    2: string chunkHash
}

typedef list<Chunk> Chunks 

struct Replication
{
    1: Chunk chunk,
    2: i32 idServer
}

service DistributionService
{
    list<Replication>  MakeReplications(1: Chunks chunks);

    list<Replication>  GetReplications(1: i32 idMetadata);
}

//thrift --gen netstd --out api idl/distribution-service.thrift