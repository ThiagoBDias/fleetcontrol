
namespace FleetControl.Api.Application.DTOs.Minas;

public class MinaResponse
{
    public long Id { get; set; }

    public long EmpresaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Codigo { get; set; }

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}

