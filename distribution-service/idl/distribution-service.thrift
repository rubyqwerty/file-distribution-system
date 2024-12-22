namespace netstd distribution_service

typedef list<Chunk> Chunks 

service DistributionService
{
    void  MakeReplications(1: i32 idMetadata);
}

//thrift --gen netstd --out distribution-service/api distribution-service/idl/distribution-service.thrift