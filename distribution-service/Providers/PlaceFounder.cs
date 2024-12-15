/// <summary>
/// Класс отвечающие за хранение группы узлов
/// </summary>
class GroupNodes
{
    public GroupNodes(int sizeGroup)
    {
        _sizeGroup = sizeGroup;
        _group = new();
        Placement = [];
    }

    /// <summary>
    /// Добавить узел в группу узлов
    /// </summary>
    /// <param name="node"></param>
    public void AddNode(ICircleNode node)
    {
        if (_group.Count() == _sizeGroup)
        {
            _group.Pop();
        }

        _group.Push(node);
    }

    /// <summary>
    /// Добавить несколько узлов в группу узлов
    /// </summary>
    /// <param name="nodes"></param>
    public void AddGroupNode(List<ICircleNode> nodes)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }
    }

    /// <summary>
    /// Добавить чанк в каждый из узлов группы
    /// </summary>
    /// <param name="placement"></param>
    /// <param name="chunk"></param>
    public void AddInGroup(ICircleNode chunk)
    {
        foreach (var node in _group)
        {

            if (Placement.ContainsKey(node))
            {
                Placement[node].Add(chunk);
                continue;
            }

            Placement.Add(node, [chunk]);
        }
    }

    Stack<ICircleNode> _group;
    private readonly int _sizeGroup;
    public Dictionary<ICircleNode, List<ICircleNode>> Placement { get; set; }
}

/// <summary>
/// Класс, отвечающий за расчет местоположений чанков
/// </summary>
public class PlaceFounder
{
    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="serverNodes"></param>
    /// <param name="chunkNodes"></param>
    /// <param name="numberOfReplication"></param>
    public PlaceFounder(List<ICircleNode> serverNodes, List<ICircleNode> chunkNodes, int numberOfReplication)
    {
        var comparer = Comparer<ICircleNode>.Create((p1, p2) => p1.hash.CompareTo(p2.hash));

        _serverNodes = new(serverNodes, comparer);

        _chunkNodes = new(chunkNodes, comparer);

        Placement = [];

        _numberOfReplications = numberOfReplication;
    }

    /// <summary>
    /// Рассчитать местоположения
    /// </summary>
    public void ComputePlacement()
    {


        GroupNodes group = new(_numberOfReplications);

        group.AddGroupNode(_serverNodes.TakeLast(_numberOfReplications).ToList());

        foreach (var chunk in _chunkNodes)
        {
            _serverNodes.Add(chunk);
        }

        foreach (var node in _serverNodes)
        {
            if (node.role == ICircleNode.Role.SRVNODE)
            {
                group.AddNode(node);
                continue;
            }

            group.AddInGroup(node);
        }

        MakePlacement(group.Placement);
    }

    /// <summary>
    /// Сформировать структуру - кто-куда
    /// </summary>
    /// <param name="places"></param>
    private void MakePlacement(Dictionary<ICircleNode, List<ICircleNode>> places)
    {
        foreach (var place in places)
        {
            foreach (var chunk in place.Value)
            {
                Placement.Add(new Placement()
                {
                    virtualServerNode = place.Key,
                    chunk = chunk
                });
            }
        }
    }

    private readonly SortedSet<ICircleNode> _serverNodes;
    private readonly SortedSet<ICircleNode> _chunkNodes;
    private readonly int _numberOfReplications;
    public List<Placement> Placement { get; set; }
}