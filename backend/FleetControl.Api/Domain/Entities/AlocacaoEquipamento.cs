namespace FleetControl.Api.Domain.Entities;

public class AlocacaoEquipamento
{
    public long Id { get; set; }

    public long LocacaoId { get; set; }

    public long EquipamentoId { get; set; }

    public string Funcao { get; set; } = string.Empty;

    public DateTime Inicio { get; set; }

    public DateTime? Fim { get; set; }

    public string? MotivoSaida { get; set; }

    public DateTime CriadoEm { get; set; }

    // Relacionamentos
    public Locacao Locacao { get; set; } = null!;

    public Equipamento Equipamento { get; set; } = null!;

    public ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();
}