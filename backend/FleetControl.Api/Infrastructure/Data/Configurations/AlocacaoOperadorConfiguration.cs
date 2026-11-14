
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class AlocacaoOperadorConfiguration
    : IEntityTypeConfiguration<AlocacaoOperador>
{
    public void Configure(EntityTypeBuilder<AlocacaoOperador> entity)
    {
        entity.ToTable("alocacao_operador");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.OperadorId)
            .HasColumnName("operador_id");

        entity.Property(e => e.EquipamentoId)
            .HasColumnName("equipamento_id");

        entity.Property(e => e.Inicio)
            .HasColumnName("inicio");

        entity.Property(e => e.Fim)
            .HasColumnName("fim");

        entity.Property(e => e.MotivoSaida)
            .HasColumnName("motivo_saida")
            .HasColumnType("text");

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        entity.HasIndex(e => e.OperadorId);

        entity.HasIndex(e => e.EquipamentoId);


// Relacionamento com Operador
entity.HasOne(e => e.Operador)
    .WithMany(e => e.Alocacoes)
    .HasForeignKey(e => e.OperadorId)
    .OnDelete(DeleteBehavior.NoAction);

// Relacionamento com Equipamento
entity.HasOne(e => e.Equipamento)
    .WithMany(e => e.AlocacoesOperadores)
    .HasForeignKey(e => e.EquipamentoId)
    .OnDelete(DeleteBehavior.NoAction);

    }
}
