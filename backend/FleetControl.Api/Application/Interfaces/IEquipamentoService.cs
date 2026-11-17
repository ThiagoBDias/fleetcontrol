
using FleetControl.Api.Application.DTOs.Equipamentos;

namespace FleetControl.Api.Application.Interfaces;

public interface IEquipamentoService
{
    Task<IEnumerable<EquipamentoResponse>> ListarAsync();

    Task<EquipamentoResponse?> BuscarPorIdAsync(long id);

    Task<EquipamentoResponse> CriarAsync(
        CriarEquipamentoRequest request);
}
