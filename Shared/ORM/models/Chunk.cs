using System;
using System.Collections.Generic;

namespace models;

public partial class Chunk
{
    public int IdMetadata { get; set; }

    public string Hash { get; set; } = null!;

    public int Position { get; set; }

    public int Size { get; set; }

    public virtual Metadata IdMetadataNavigation { get; set; } = null!;

    public virtual ICollection<Replication> Replications { get; set; } = new List<Replication>();
}
