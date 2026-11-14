
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetControl.Api.Infrastructure.Data.Configurations;

public class OperadorConfiguration : IEntityTypeConfiguration<Operador>
{
    public void Configure(EntityTypeBuilder<Operador> entity)
    {
        entity.ToTable("operador");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.MinaId)
            .HasColumnName("mina_id");

        entity.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Matricula)
            .HasColumnName("matricula")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.Cartao)
            .HasColumnName("cartao")
            .HasMaxLength(50);

        entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("ATIVO")
            .IsRequired();

        entity.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        // Matrícula única por mina
        entity.HasIndex(e => new { e.MinaId, e.Matricula })
            .IsUnique();

        // Relacionamento com Mina
        entity.HasOne(e => e.Mina)
            .WithMany(e => e.Operadores)
            .HasForeignKey(e => e.MinaId)
            .OnDelete(DeleteBehavior.NoAction);


    }
}
