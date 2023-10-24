using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mediabrandsgastosdeviajes.Models;

public partial class ViajesgastosContext : DbContext
{
    public ViajesgastosContext()
    {
    }

    public ViajesgastosContext(DbContextOptions<ViajesgastosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Viajero> Viajeros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=GWNR71517\\SQLEXPRESS;Database=viajesgastos;Integrated Security=True; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.ToTable("Presupuesto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PresupuestoGeneral).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Saldo).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Viajero>(entity =>
        {
            entity.ToTable("Viajero");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Agencia).HasMaxLength(50);
            entity.Property(e => e.Alimentacion).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Hotel).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Otros).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalGastos).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Transportes).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
