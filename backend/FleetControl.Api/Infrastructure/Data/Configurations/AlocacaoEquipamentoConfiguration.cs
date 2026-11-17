
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class AlocacaoEquipamentoConfiguration
    : IEntityTypeConfiguration<AlocacaoEquipamento>
{
    public void Configure(EntityTypeBuilder<AlocacaoEquipamento> entity)
    {
        entity.ToTable("alocacao_equipamento");

        // Chave primária
        entity.HasKey(e => e.Id);

        // Propriedades
        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.LocacaoId)
            .HasColumnName("locacao_id");

        entity.Property(e => e.EquipamentoId)
            .HasColumnName("equipamento_id");

        entity.Property(e => e.Funcao)
            .HasColumnName("funcao")
            .HasMaxLength(20)
            .IsRequired();

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

        // Chave alternativa para relacionamento com viagem
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.LocacaoId
        });

        // Índices
        entity.HasIndex(e => e.EquipamentoId)
            .HasDatabaseName("idx_alocacao_equipamento_equipamento");

        entity.HasIndex(e => e.LocacaoId)
            .HasDatabaseName("idx_alocacao_equipamento_locacao");

        entity.HasIndex(e => new { e.Inicio, e.Fim })
            .HasDatabaseName("idx_alocacao_equipamento_periodo");

        // Relacionamento com Locação
        entity.HasOne(e => e.Locacao)
            .WithMany(e => e.AlocacoesEquipamentos)
            .HasForeignKey(e => e.LocacaoId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Equipamento
        entity.HasOne(e => e.Equipamento)
            .WithMany(e => e.Alocacoes)
            .HasForeignKey(e => e.EquipamentoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
