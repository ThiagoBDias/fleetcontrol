
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class ViagemConfiguration : IEntityTypeConfiguration<Viagem>
{
    public void Configure(EntityTypeBuilder<Viagem> entity)
    {
        entity.ToTable("viagem");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.Codigo)
            .HasColumnName("codigo")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        entity.Property(e => e.LocacaoId)
            .HasColumnName("locacao_id");

        entity.Property(e => e.AlocacaoEquipamentoId)
            .HasColumnName("alocacao_equipamento_id");

        entity.Property(e => e.OrigemPoligonoId)
            .HasColumnName("origem_poligono_id");

        entity.Property(e => e.DestinoPoligonoId)
            .HasColumnName("destino_poligono_id");

        entity.Property(e => e.LitologiaId)
            .HasColumnName("litologia_id");

        entity.Property(e => e.EquipamentoCargaId)
            .HasColumnName("equipamento_carga_id");

        entity.Property(e => e.Massa)
            .HasColumnName("massa")
            .HasPrecision(12, 2);

        entity.Property(e => e.Inicio)
            .HasColumnName("inicio")
            .HasDefaultValueSql("NOW()");

        entity.Property(e => e.Fim)
            .HasColumnName("fim");

        entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(30)
            .HasDefaultValue("EM_ANDAMENTO")
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Código UUID único
        entity.HasIndex(e => e.Codigo)
            .IsUnique();
    }
}
