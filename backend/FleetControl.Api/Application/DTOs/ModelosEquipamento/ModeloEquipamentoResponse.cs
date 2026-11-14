
namespace FleetControl.Api.Application.DTOs.ModelosEquipamento;

public class ModeloEquipamentoResponse
{
    public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public bool Ativo { get; set; }
}
