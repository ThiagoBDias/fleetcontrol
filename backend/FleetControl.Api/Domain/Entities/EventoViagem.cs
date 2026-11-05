namespace FleetControl.Api.Domain.Entities;

public class EventoViagem
{
    public long Id { get; set; }

    public long ViagemId { get; set; }

    public string TipoEvento { get; set; } = string.Empty;

    public DateTime DataHora { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public long? PoligonoId { get; set; }

    public long? OperadorId { get; set; }

    public long? EquipamentoId { get; set; }

    public string? Observacao { get; set; }

    public DateTime CriadoEm { get; set; }

    // Relacionamentos
    public Viagem Viagem { get; set; } = null!;

    public Poligono? Poligono { get; set; }

    public Operador? Operador { get; set; }

    public Equipamento? Equipamento { get; set; }
}