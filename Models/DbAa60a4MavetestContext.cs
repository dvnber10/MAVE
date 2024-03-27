using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Models;

public partial class DbAa60a4MavetestContext : DbContext
{
    public DbAa60a4MavetestContext()
    {
    }

    public DbAa60a4MavetestContext(DbContextOptions<DbAa60a4MavetestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserModel> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Base");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("User");

            entity.HasKey(s => s.Id);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.NameU)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pass).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
