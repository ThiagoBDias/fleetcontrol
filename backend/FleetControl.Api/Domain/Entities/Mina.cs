namespace FleetControl.Api.Domain.Entities;

public class Mina
{
    public long Id { get; set; }

    public long EmpresaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Codigo { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }
}