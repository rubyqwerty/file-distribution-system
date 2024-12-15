namespace chunk;

public interface IChunkProvider
{
    internal abstract List<CircleNode> GetChunkNodes();
}

public class MockChunk : IChunkProvider
{
    public List<CircleNode> GetChunkNodes()
    {
        return
        [
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "853688e106d5a0c6ae33938adf05b053"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "b691b95c4c42b0e2b667ff7962ff4888"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "e5091bada4e69368e2b2d8461c45f1c1"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "2f9ec3fcac67baf8bee926d7b2300951"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "3196d779d18a78fa0d27fafa8fa17207"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "862aec6a928c1480f14215cfffce7957"},
            new CircleNode {role = CircleNode.Role.CHUNK, hash = "e1670cbaff36c1c4c86edcc881bb35d3"},
        ];
    }
}