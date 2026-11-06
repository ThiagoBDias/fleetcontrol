
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class EquipamentoConfiguration
    : IEntityTypeConfiguration<Equipamento>
{
    public void Configure(EntityTypeBuilder<Equipamento> entity)
    {
        entity.ToTable("equipamento");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.MinaId)
            .HasColumnName("mina_id")
            .IsRequired();

        entity.Property(e => e.ModeloId)
            .HasColumnName("modelo_id")
            .IsRequired();

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
            .HasDefaultValue("ATIVO")
            .IsRequired();

        entity.Property(e => e.PossuiBalanca)
            .HasColumnName("possui_balanca")
            .HasDefaultValue(false);

        entity.Property(e => e.Bloqueado)
            .HasColumnName("bloqueado")
            .HasDefaultValue(false);

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Código único por mina
        entity.HasIndex(e => new { e.MinaId, e.Codigo })
            .IsUnique();

        // Chave alternativa para relacionamentos compostos
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.MinaId
        });
    }
}
