
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
{
    public void Configure(EntityTypeBuilder<Locacao> entity)
    {
        entity.ToTable("locacao");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.MinaId)
            .HasColumnName("mina_id");

        entity.Property(e => e.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.OrigemPoligonoId)
            .HasColumnName("origem_poligono_id");

        entity.Property(e => e.DestinoPoligonoId)
            .HasColumnName("destino_poligono_id");

        entity.Property(e => e.Inicio)
            .HasColumnName("inicio")
            .HasDefaultValueSql("NOW()");

        entity.Property(e => e.Fim)
            .HasColumnName("fim");

        entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("ATIVA")
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Código único por mina
        entity.HasIndex(e => new { e.MinaId, e.Codigo })
            .IsUnique();

        // Chave alternativa para relacionamento com viagem
        entity.HasAlternateKey(e => new
        {
            e.Id,
            e.OrigemPoligonoId,
            e.DestinoPoligonoId
        });


        // Relacionamento com Mina
        entity.HasOne(e => e.Mina)
            .WithMany(e => e.Locacoes)
            .HasForeignKey(e => e.MinaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Polígono de Origem
        entity.HasOne(e => e.OrigemPoligono)
            .WithMany(e => e.LocacoesOrigem)
            .HasForeignKey(e => new
            {
                e.OrigemPoligonoId,
                e.MinaId
            })
            .HasPrincipalKey(e => new
            {
                e.Id,
                e.MinaId
            })
            .OnDelete(DeleteBehavior.NoAction);

        // Relacionamento com Polígono de Destino
        entity.HasOne(e => e.DestinoPoligono)
            .WithMany(e => e.LocacoesDestino)
            .HasForeignKey(e => new
            {
                e.DestinoPoligonoId,
                e.MinaId
            })
            .HasPrincipalKey(e => new
            {
                e.Id,
                e.MinaId
            })
            .OnDelete(DeleteBehavior.NoAction);

    }
}
