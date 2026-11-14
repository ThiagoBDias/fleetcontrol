
using FleetControl.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Infrastructure.Data;

public class FleetControlDbContext : DbContext
{
    public FleetControlDbContext(
        DbContextOptions<FleetControlDbContext> options)
        : base(options)
    {
    }

    // Cadastros
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Mina> Minas => Set<Mina>();
    public DbSet<ModeloEquipamento> ModelosEquipamento => Set<ModeloEquipamento>();
    public DbSet<Equipamento> Equipamentos => Set<Equipamento>();
    public DbSet<Operador> Operadores => Set<Operador>();

    // Geografia e materiais
    public DbSet<Poligono> Poligonos => Set<Poligono>();
    public DbSet<Material> Materiais => Set<Material>();
    public DbSet<Litologia> Litologias => Set<Litologia>();

    // Operação
    public DbSet<Locacao> Locacoes => Set<Locacao>();
    public DbSet<AlocacaoEquipamento> AlocacoesEquipamentos => Set<AlocacaoEquipamento>();
    public DbSet<AlocacaoOperador> AlocacoesOperadores => Set<AlocacaoOperador>();

    // Transporte
    public DbSet<Viagem> Viagens => Set<Viagem>();
    public DbSet<EventoViagem> EventosViagem => Set<EventoViagem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FleetControlDbContext).Assembly);
    }
}
