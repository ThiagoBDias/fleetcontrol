
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class ModeloEquipamentoConfiguration
    : IEntityTypeConfiguration<ModeloEquipamento>
{
    public void Configure(EntityTypeBuilder<ModeloEquipamento> entity)
    {
        entity.ToTable("modelo_equipamento");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        entity.HasIndex(e => e.Nome)
            .IsUnique();
    }
}
