
namespace FleetControl.Api.Application.DTOs.Empresas;

public class EmpresaResponse
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}
