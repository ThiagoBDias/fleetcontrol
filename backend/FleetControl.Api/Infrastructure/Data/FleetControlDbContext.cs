using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Infrastructure.Data;

public class FleetControlDbContext : DbContext
{

    public FleetControlDbContext(DbContextOptions<FleetControlDbContext> options)
        : base(options)
    {
    }

    public DbSet<Equipamento> Equipamentos => Set<Equipamento>();

    public DbSet<Mina> Minas => Set<Mina>();

    public DbSet<Empresa> Empresas => Set<Empresa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.ToTable("equipamento");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.MinaId)
                .HasColumnName("mina_id");

            entity.Property(e => e.ModeloId)
                .HasColumnName("modelo_id");

            entity.Property(e => e.Codigo)
                .HasColumnName("codigo")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.PossuiBalanca)
                .HasColumnName("possui_balanca");

            entity.Property(e => e.Bloqueado)
                .HasColumnName("bloqueado");

            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em");

            modelBuilder.Entity<Mina>(entity =>
{
entity.ToTable("mina");

entity.HasKey(e => e.Id);

entity.Property(e => e.Id)
    .HasColumnName("id");

entity.Property(e => e.EmpresaId)
    .HasColumnName("empresa_id");

entity.Property(e => e.Nome)
    .HasColumnName("nome")
    .HasMaxLength(150)
    .IsRequired();

entity.Property(e => e.Codigo)
    .HasColumnName("codigo")
    .HasMaxLength(50);

entity.Property(e => e.Ativo)
    .HasColumnName("ativo")
    .IsRequired();

entity.Property(e => e.CriadoEm)
    .HasColumnName("criado_em");
});

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.ToTable("empresa");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Ativo)
                    .HasColumnName("ativo")
                    .IsRequired();

                entity.Property(e => e.CriadoEm)
                    .HasColumnName("criado_em");
            });
        });
    }
}