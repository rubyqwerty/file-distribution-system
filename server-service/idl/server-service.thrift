namespace netstd server_service

struct VirtualNode
{
    1: string serverAddress,
    2: string hash
}

struct Server
{
    1: i32 idServer,
    2: string address
}

service ServerService
{
    Server GetServer(1: i32 idServer);
    list<VirtualNode> GetVirtualNodes();
}
//thrift --gen netstd --out api idl/server-service.thrift