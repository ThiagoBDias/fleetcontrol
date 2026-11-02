namespace FleetControl.Api.Domain.Entities;

public class Operador
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Matricula { get; set; } = string.Empty;

    public string? Cartao { get; set; }

    public string Status { get; set; } = "ATIVO";

    public DateTime CriadoEm { get; set; }
}