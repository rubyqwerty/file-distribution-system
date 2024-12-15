namespace nodes;

public interface IServerNodesProvider
{
    internal abstract List<ICircleNode> GetServerNodes();
}

public class MockNodes : IServerNodesProvider
{
    public List<ICircleNode> GetServerNodes()
    {

        return
        [
            new CircleServerNode { hash = "f528764d624db129b32c21fbca0cb8d6"},
            new CircleServerNode {hash = "ab416c39d509e72c5a0a7451a45bc65e"},
            new CircleServerNode {hash = "94084e434024aa1b2db3b06c7e4fa0f1"},
            new CircleServerNode{ hash = "588f4773f2207d56dd53d5c3e2018339"},
            new CircleServerNode { hash = "a46d1ee667058573d4ebb750449a44b1"},
        ];
    }
}