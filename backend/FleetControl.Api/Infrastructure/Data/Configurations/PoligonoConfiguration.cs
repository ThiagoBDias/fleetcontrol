
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class PoligonoConfiguration
    : IEntityTypeConfiguration<Poligono>
{
    public void Configure(EntityTypeBuilder<Poligono> entity)
    {
        entity.ToTable("poligono");

        // Chave primária
        entity.HasKey(e => e.Id);

        // Propriedades
        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.MinaId)
            .HasColumnName("mina_id");

        entity.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(e => e.Geometria)
            .HasColumnName("geometria")
            .HasColumnType("geometry(Polygon, 4326)")
            .IsRequired();

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Chave alternativa
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.MinaId
        });

        // Índice espacial PostGIS
        entity.HasIndex(e => e.Geometria)
            .HasMethod("GIST")
            .HasDatabaseName("idx_poligono_geometria");

        // Índice composto por mina e tipo
        entity.HasIndex(e => new { e.MinaId, e.Tipo })
            .HasDatabaseName("idx_poligono_mina_tipo");

        // Relacionamento com Mina
        entity.HasOne(e => e.Mina)
            .WithMany(e => e.Poligonos)
            .HasForeignKey(e => e.MinaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
