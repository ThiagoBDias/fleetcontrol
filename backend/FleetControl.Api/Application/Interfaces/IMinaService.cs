
using FleetControl.Api.Application.DTOs.Minas;

namespace FleetControl.Api.Application.Interfaces;

public interface IMinaService
{
    Task<IEnumerable<MinaResponse>> ListarAsync();

    Task<MinaResponse?> BuscarPorIdAsync(long id);

    Task<MinaResponse> CriarAsync(CriarMinaRequest request);
}
