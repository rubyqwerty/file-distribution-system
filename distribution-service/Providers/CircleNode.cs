public abstract class ICircleNode
{
    public enum Role
    {
        CHUNK = 1,
        SRVNODE = 2
    }

    public Role role { set; get; }
    required public string hash { set; get; }

    public override string ToString()
    {
        return $"{role.ToString()} {hash}";
    }
}

public class CircleServerNode : ICircleNode
{
    public CircleServerNode()
    {
        role = Role.SRVNODE;
    }
}

public class CircleChunkNode : ICircleNode
{
    public CircleChunkNode()
    {
        role = Role.CHUNK;
    }
}

public struct Placement
{
    public ICircleNode virtualServerNode;
    public ICircleNode chunk;
}