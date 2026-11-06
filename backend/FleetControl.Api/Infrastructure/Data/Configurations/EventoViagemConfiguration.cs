
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class EventoViagemConfiguration
    : IEntityTypeConfiguration<EventoViagem>
{
    public void Configure(EntityTypeBuilder<EventoViagem> entity)
    {
        entity.ToTable("evento_viagem");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.ViagemId)
            .HasColumnName("viagem_id");

        entity.Property(e => e.TipoEvento)
            .HasColumnName("tipo_evento")
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(e => e.DataHora)
            .HasColumnName("data_hora")
            .HasDefaultValueSql("NOW()");

        entity.Property(e => e.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(10, 7);

        entity.Property(e => e.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(10, 7);

        entity.Property(e => e.PoligonoId)
            .HasColumnName("poligono_id");

        entity.Property(e => e.OperadorId)
            .HasColumnName("operador_id");

        entity.Property(e => e.EquipamentoId)
            .HasColumnName("equipamento_id");

        entity.Property(e => e.Observacao)
            .HasColumnName("observacao")
            .HasColumnType("text");

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        entity.HasIndex(e => e.ViagemId);
        entity.HasIndex(e => e.PoligonoId);
        entity.HasIndex(e => e.OperadorId);
        entity.HasIndex(e => e.EquipamentoId);
    }
}
