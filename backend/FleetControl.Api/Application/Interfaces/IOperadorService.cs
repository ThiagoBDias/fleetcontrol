
using FleetControl.Api.Application.DTOs.Operadores;

namespace FleetControl.Api.Application.Interfaces;

public interface IOperadorService
{
    Task<IEnumerable<OperadorResponse>> ListarAsync();

    Task<OperadorResponse?> BuscarPorIdAsync(long id);

    Task<OperadorResponse> CriarAsync(
        CriarOperadorRequest request);
}
