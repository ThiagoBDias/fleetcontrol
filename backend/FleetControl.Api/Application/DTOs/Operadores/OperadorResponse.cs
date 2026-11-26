
namespace FleetControl.Api.Application.DTOs.Operadores;

public class OperadorResponse
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Matricula { get; set; } = string.Empty;

    public string? Cartao { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}
