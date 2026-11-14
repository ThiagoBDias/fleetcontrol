
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


        // Relacionamento com Locação
        entity.HasOne(e => e.Locacao)
            .WithMany(e => e.Viagens)
            .HasForeignKey(e => new
            {
                e.LocacaoId,
                e.OrigemPoligonoId,
                e.DestinoPoligonoId
            })
            .HasPrincipalKey(e => new
            {
                e.Id,
                e.OrigemPoligonoId,
                e.DestinoPoligonoId
            })
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Alocação de Equipamento
        entity.HasOne(e => e.AlocacaoEquipamento)
            .WithMany(e => e.Viagens)
            .HasForeignKey(e => new
            {
                e.AlocacaoEquipamentoId,
                e.LocacaoId
            })
            .HasPrincipalKey(e => new
            {
                e.Id,
                e.LocacaoId
            })
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Polígono de Origem
        entity.HasOne(e => e.OrigemPoligono)
            .WithMany()
            .HasForeignKey(e => e.OrigemPoligonoId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Polígono de Destino
        entity.HasOne(e => e.DestinoPoligono)
            .WithMany()
            .HasForeignKey(e => e.DestinoPoligonoId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Litologia
        entity.HasOne(e => e.Litologia)
            .WithMany(e => e.Viagens)
            .HasForeignKey(e => e.LitologiaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Equipamento de Carga
        entity.HasOne(e => e.EquipamentoCarga)
            .WithMany(e => e.Viagens)
            .HasForeignKey(e => e.EquipamentoCargaId)
            .OnDelete(DeleteBehavior.NoAction);


    }
}
