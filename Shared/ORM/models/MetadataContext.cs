using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace models;

public partial class MetadataContext : DbContext
{
    public MetadataContext()
    {
    }

    public MetadataContext(DbContextOptions<MetadataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chunk> Chunks { get; set; }

    public virtual DbSet<Metadata> Metadata { get; set; }

    public virtual DbSet<Replication> Replications { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<VirtualNode> VirtualNodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=../../DB/metadata.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chunk>(entity =>
        {
            entity.HasKey(e => new { e.IdMetadata, e.Hash });

            entity.ToTable("Chunk");

            entity.HasIndex(e => e.Hash, "IX_Chunk_Hash").IsUnique();

            entity.Property(e => e.IdMetadata)
                .HasColumnType("integer(10)")
                .HasColumnName("Id_metadata");
            entity.Property(e => e.Hash).HasColumnType("varchar(255)");
            entity.Property(e => e.Position).HasColumnType("integer(10)");
            entity.Property(e => e.Size).HasColumnType("integer(10)");

            entity.HasOne(d => d.IdMetadataNavigation).WithMany(p => p.Chunks)
                .HasForeignKey(d => d.IdMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Metadata>(entity =>
        {
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("'0'")
                .HasColumnType("timestamp")
                .HasColumnName("Creation_date");
            entity.Property(e => e.ModificationDate)
                .HasDefaultValueSql("'0'")
                .HasColumnType("timestamp")
                .HasColumnName("Modification_date");
            entity.Property(e => e.Name).HasColumnType("varchar(255)");
            entity.Property(e => e.Size).HasColumnType("integer(10)");
        });

        modelBuilder.Entity<Replication>(entity =>
        {
            entity.HasKey(e => new { e.IdServer, e.HashChunk, e.IdMetadata });

            entity.ToTable("Replication");

            entity.Property(e => e.IdServer)
                .HasColumnType("integer(10)")
                .HasColumnName("Id_server");
            entity.Property(e => e.HashChunk)
                .HasColumnType("varchar(255)")
                .HasColumnName("Hash_chunk");
            entity.Property(e => e.IdMetadata)
                .HasColumnType("integer(10)")
                .HasColumnName("Id_metadata");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp")
                .HasColumnName("Creation_date");

            // entity.HasOne(d => d.IdServerNavigation).WithMany(p => p.Replications)
            //      .HasForeignKey(d => d.IdServer)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Chunk).WithMany(p => p.Replications)
                .HasForeignKey(d => new { d.IdMetadata, d.HashChunk })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.ToTable("Server");

            entity.Property(e => e.Address).HasColumnType("varchar(255)");
            entity.Property(e => e.Memory).HasColumnType("integer(10)");
            entity.Property(e => e.Priority).HasColumnType("integer(10)");
        });

        modelBuilder.Entity<VirtualNode>(entity =>
        {
            entity.HasKey(e => new { e.IdServer, e.Hash });

            entity.ToTable("Virtual_node");

            entity.HasIndex(e => e.Hash, "IX_Virtual_node_Hash").IsUnique();

            entity.Property(e => e.IdServer)
                .HasColumnType("integer(10)")
                .HasColumnName("Id_server");
            entity.Property(e => e.Hash).HasColumnType("varchar(255)");

            //   entity.HasOne(d => d.IdServerNavigation).WithMany(p => p.VirtualNodes)
            //   .HasForeignKey(d => d.IdServer)
            //  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
