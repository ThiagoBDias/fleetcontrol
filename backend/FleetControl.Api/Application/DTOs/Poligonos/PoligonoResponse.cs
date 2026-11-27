
namespace FleetControl.Api.Application.DTOs.Poligonos;

public class PoligonoResponse
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public double[][] Coordenadas { get; set; } = [];

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}
