using System;
using System.Collections.Generic;

namespace models;

public partial class VirtualNode
{
    public int IdServer { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Server IdServerNavigation { get; set; } = null!;
}
