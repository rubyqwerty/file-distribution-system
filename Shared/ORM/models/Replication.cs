using System;
using System.Collections.Generic;

namespace models;

public partial class Replication
{
    public int IdServer { get; set; }

    public string HashChunk { get; set; } = null!;

    public int IdMetadata { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Chunk Chunk { get; set; } = null!;

    public virtual Server IdServerNavigation { get; set; } = null!;
}
