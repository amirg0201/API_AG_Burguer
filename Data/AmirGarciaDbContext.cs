using System;
using System.Collections.Generic;
using API_AG_Burguer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API_AG_Burguer.Data;

public partial class AmirGarciaDbContext : DbContext
{
    public AmirGarciaDbContext()
    {
    }

    public AmirGarciaDbContext(DbContextOptions<AmirGarciaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Burger> Burgers { get; set; }

    public virtual DbSet<Promo> Promos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AmirGarciaDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Burger>(entity =>
        {
            entity.ToTable("Burger");

            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Promo>(entity =>
        {
            entity.ToTable("Promo");

            entity.HasIndex(e => e.Burgerid, "IX_Promo_Burgerid");

            entity.Property(e => e.FechaPromo).HasColumnName("fechaPromo");

            entity.HasOne(d => d.Burger).WithMany(p => p.Promos).HasForeignKey(d => d.Burgerid);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
