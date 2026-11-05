namespace FleetControl.Api.Domain.Entities;

public class AlocacaoOperador
{
    public long Id { get; set; }

    public long OperadorId { get; set; }

    public long EquipamentoId { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime? Fim { get; set; }

    public string? MotivoSaida { get; set; }

    public DateTime CriadoEm { get; set; }

    // Relacionamentos
    public Operador Operador { get; set; } = null!;

    public Equipamento Equipamento { get; set; } = null!;
}