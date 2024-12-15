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
    public string address { set; get; } = "";

    public Role role { set; get; } = Role.SRVNODE;
}

public class CircleChunkNode : ICircleNode
{
    public Role role { set; get; } = Role.CHUNK;
}

public struct Placement
{
    public ICircleNode virtualServerNode;
    public ICircleNode chunk;
}