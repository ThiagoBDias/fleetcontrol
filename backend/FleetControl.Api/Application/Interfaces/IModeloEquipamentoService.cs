
using FleetControl.Api.Application.DTOs.ModelosEquipamento;

namespace FleetControl.Api.Application.Interfaces;

public interface IModeloEquipamentoService
{
    Task<IEnumerable<ModeloEquipamentoResponse>> ListarAsync();

    Task<ModeloEquipamentoResponse?> BuscarPorIdAsync(long id);

    Task<ModeloEquipamentoResponse> CriarAsync(
        CriarModeloEquipamentoRequest request);
}
