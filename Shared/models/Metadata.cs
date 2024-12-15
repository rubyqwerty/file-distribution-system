using System;
using System.Collections.Generic;

namespace models;

public partial class Metadata
{
    public int Id { get; set; }

    public int Size { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ModificationDate { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Chunk> Chunks { get; set; } = new List<Chunk>();
}
