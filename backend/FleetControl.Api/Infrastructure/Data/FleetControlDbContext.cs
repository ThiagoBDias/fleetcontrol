using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Infrastructure.Data;

public class FleetControlDbContext : DbContext
{
    public FleetControlDbContext(DbContextOptions<FleetControlDbContext> options)
        : base(options)
    {
    }
public DbSet<Empresa> Empresas => Set<Empresa>();
public DbSet<Mina> Minas => Set<Mina>();
public DbSet<ModeloEquipamento> ModelosEquipamento => Set<ModeloEquipamento>();
public DbSet<Equipamento> Equipamentos => Set<Equipamento>();
public DbSet<Operador> Operadores => Set<Operador>();
public DbSet<Poligono> Poligonos => Set<Poligono>();
public DbSet<Material> Materiais => Set<Material>();
public DbSet<Litologia> Litologias => Set<Litologia>();
public DbSet<Locacao> Locacoes => Set<Locacao>();
public DbSet<AlocacaoEquipamento> AlocacoesEquipamentos => Set<AlocacaoEquipamento>();
public DbSet<AlocacaoOperador> AlocacoesOperadores => Set<AlocacaoOperador>();
public DbSet<Viagem> Viagens => Set<Viagem>();
public DbSet<EventoViagem> EventosViagem => Set<EventoViagem>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // EQUIPAMENTO
        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.ToTable("equipamento");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.MinaId)
                .HasColumnName("mina_id");

            entity.Property(e => e.ModeloId)
                .HasColumnName("modelo_id");

            entity.Property(e => e.Codigo)
                .HasColumnName("codigo")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.PossuiBalanca)
                .HasColumnName("possui_balanca");

            entity.Property(e => e.Bloqueado)
                .HasColumnName("bloqueado");

            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em");
        });

        // MINA
        modelBuilder.Entity<Mina>(entity =>
        {
            entity.ToTable("mina");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.EmpresaId)
                .HasColumnName("empresa_id");

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Codigo)
                .HasColumnName("codigo")
                .HasMaxLength(50);

            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em");
            entity.HasIndex(e => new { e.EmpresaId, e.Codigo })
    .IsUnique();

entity.HasAlternateKey(e => new
{
    e.Id,
    e.EmpresaId
});
        });

        // EMPRESA
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("empresa");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            entity.Property(e => e.CriadoEm)
                .HasColumnName("criado_em");
        });

        // MODELO DE EQUIPAMENTO
        modelBuilder.Entity<ModeloEquipamento>(entity =>
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
                .IsRequired();

            entity.HasIndex(e => e.Nome)
                .IsUnique();
        });
        // OPERADOR
modelBuilder.Entity<Operador>(entity =>
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
        .IsRequired();

    entity.Property(e => e.CriadoEm)
        .HasColumnName("criado_em");

    entity.HasIndex(e => new { e.MinaId, e.Matricula })
        .IsUnique();
});
// POLIGONO
modelBuilder.Entity<Poligono>(entity =>
{
    entity.ToTable("poligono");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.Id)
        .HasColumnName("id");

    entity.Property(e => e.MinaId)
        .HasColumnName("mina_id");

    entity.Property(e => e.Nome)
        .HasColumnName("nome")
        .HasMaxLength(150)
        .IsRequired();

    entity.Property(e => e.Tipo)
        .HasColumnName("tipo")
        .HasMaxLength(30)
        .IsRequired();

    entity.Property(e => e.Geometria)
        .HasColumnName("geometria")
        .HasColumnType("geometry(Polygon, 4326)")
        .IsRequired();

    entity.Property(e => e.Ativo)
        .HasColumnName("ativo")
        .IsRequired();

    entity.Property(e => e.CriadoEm)
        .HasColumnName("criado_em");

    entity.HasIndex(e => new { e.Id, e.MinaId })
        .IsUnique();
});
// MATERIAL
modelBuilder.Entity<Material>(entity =>
{
    entity.ToTable("material");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.Id)
        .HasColumnName("id");

    entity.Property(e => e.MinaId)
        .HasColumnName("mina_id");

    entity.Property(e => e.Nome)
        .HasColumnName("nome")
        .HasMaxLength(150)
        .IsRequired();

    entity.Property(e => e.Ativo)
        .HasColumnName("ativo")
        .IsRequired();

    entity.Property(e => e.CriadoEm)
        .HasColumnName("criado_em");

    entity.HasIndex(e => new { e.MinaId, e.Nome })
        .IsUnique();

    entity.HasIndex(e => new { e.Id, e.MinaId })
        .IsUnique();
});

// LITOLOGIA
modelBuilder.Entity<Litologia>(entity =>
{
    entity.ToTable("litologia");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.Id)
        .HasColumnName("id");

    entity.Property(e => e.MinaId)
        .HasColumnName("mina_id");

    entity.Property(e => e.MaterialId)
        .HasColumnName("material_id");

    entity.Property(e => e.Nome)
        .HasColumnName("nome")
        .HasMaxLength(150)
        .IsRequired();

  entity.Property(e => e.Tipo)
    .HasColumnName("tipo")
    .HasMaxLength(100);

    entity.Property(e => e.Ativo)
        .HasColumnName("ativo")
        .IsRequired();

    entity.Property(e => e.CriadoEm)
        .HasColumnName("criado_em");

    entity.HasIndex(e => new { e.MinaId, e.Nome })
        .IsUnique();

    entity.HasIndex(e => new { e.Id, e.MinaId })
        .IsUnique();
});
// LOCACAO
modelBuilder.Entity<Locacao>(entity =>
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
        .HasColumnName("inicio");

    entity.Property(e => e.Fim)
        .HasColumnName("fim");

    entity.Property(e => e.Status)
        .HasColumnName("status")
        .HasMaxLength(20)
        .IsRequired();

    entity.Property(e => e.CriadoEm)
        .HasColumnName("criado_em");

    entity.HasIndex(e => new { e.MinaId, e.Codigo })
        .IsUnique();

    entity.HasAlternateKey(e => new
    {
        e.Id,
        e.OrigemPoligonoId,
        e.DestinoPoligonoId
    });
});
// ALOCACAO DE EQUIPAMENTO
modelBuilder.Entity<AlocacaoEquipamento>(entity =>
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
        .HasColumnName("criado_em");

    entity.HasAlternateKey(e => new
    {
        e.Id,
        e.LocacaoId
    });

    entity.HasIndex(e => e.EquipamentoId);

    entity.HasIndex(e => e.LocacaoId);

    entity.HasIndex(e => new
    {
        e.Inicio,
        e.Fim
    });
});
// ALOCACAO DE OPERADOR
modelBuilder.Entity<AlocacaoOperador>(entity =>
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
        .HasColumnName("criado_em");

    entity.HasIndex(e => e.OperadorId);

    entity.HasIndex(e => e.EquipamentoId);

    entity.HasIndex(e => new
    {
        e.Inicio,
        e.Fim
    });
});


    }
}