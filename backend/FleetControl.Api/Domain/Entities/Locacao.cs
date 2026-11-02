namespace FleetControl.Api.Domain.Entities;

public class Locacao
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public long OrigemPoligonoId { get; set; }

    public long DestinoPoligonoId { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime? Fim { get; set; }

    public string Status { get; set; } = "ATIVA";

    public DateTime CriadoEm { get; set; }
}