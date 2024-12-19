using System;
using System.Collections.Generic;

namespace models;

public partial class Server
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public int Memory { get; set; }

    public int Priority { get; set; }

    //  public virtual ICollection<Replication> Replications { get; set; } = new List<Replication>();

    //public virtual ICollection<VirtualNode> VirtualNodes { get; set; } = new List<VirtualNode>();
}
