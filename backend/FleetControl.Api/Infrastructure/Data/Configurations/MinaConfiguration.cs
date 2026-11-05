
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class MinaConfiguration : IEntityTypeConfiguration<Mina>
{
    public void Configure(EntityTypeBuilder<Mina> entity)
    {
        entity.ToTable("mina");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.EmpresaId)
            .HasColumnName("empresa_id")
            .IsRequired();

        entity.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(50);

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Código único dentro da empresa
        entity.HasIndex(e => new { e.EmpresaId, e.Codigo })
            .IsUnique();

        // Chave alternativa para relacionamentos compostos
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.EmpresaId
        });
    }
}
