
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

        entity.HasKey(e => e.Id);

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
            .HasColumnName("inicio");

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

        entity.HasIndex(e => e.EquipamentoId);
        entity.HasIndex(e => e.LocacaoId);

        entity.HasIndex(e => new { e.Inicio, e.Fim });
    }
}
