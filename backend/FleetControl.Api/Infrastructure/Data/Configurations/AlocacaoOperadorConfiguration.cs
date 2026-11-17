
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

        // Chave primária
        entity.HasKey(e => e.Id);

        // Propriedades
        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.OperadorId)
            .HasColumnName("operador_id");

        entity.Property(e => e.EquipamentoId)
            .HasColumnName("equipamento_id");

        entity.Property(e => e.Inicio)
            .HasColumnName("inicio")
            .HasDefaultValueSql("NOW()");

        entity.Property(e => e.Fim)
            .HasColumnName("fim");

        entity.Property(e => e.MotivoSaida)
            .HasColumnName("motivo_saida")
            .HasColumnType("text");

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Índices
        entity.HasIndex(e => e.OperadorId)
            .HasDatabaseName("idx_alocacao_operador_operador");

        entity.HasIndex(e => e.EquipamentoId)
            .HasDatabaseName("idx_alocacao_operador_equipamento");

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
