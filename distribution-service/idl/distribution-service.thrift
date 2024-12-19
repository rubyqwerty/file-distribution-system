namespace netstd distribution_service

struct Chunk
{
    1: i32 idMetadata,
    2: string chunkHash
}

typedef list<Chunk> Chunks 

service DistributionService
{
    void  MakeReplications(1: Chunks chunks);
}

//thrift --gen netstd --out distribution-service/api distribution-service/idl/distribution-service.thrift