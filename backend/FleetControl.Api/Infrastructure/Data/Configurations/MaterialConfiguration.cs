
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> entity)
    {
        entity.ToTable("material");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.MinaId)
            .HasColumnName("mina_id");

        entity.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Nome único dentro de cada mina
        entity.HasIndex(e => new { e.MinaId, e.Nome })
            .IsUnique();

        // Chave alternativa para relacionamentos compostos
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.MinaId
        });
    }
}

