namespace FleetControl.Api.Domain.Entities;

public class Empresa
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }
}