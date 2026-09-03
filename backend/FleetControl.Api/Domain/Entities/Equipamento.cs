namespace FleetControl.Api.Domain.Entities;

public class Equipamento
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public long ModeloId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string? Nome { get; set; }

    public string Status { get; set; } = "ATIVO";

    public bool PossuiBalanca { get; set; }

    public bool Bloqueado { get; set; }

    public DateTime CriadoEm { get; set; }
}