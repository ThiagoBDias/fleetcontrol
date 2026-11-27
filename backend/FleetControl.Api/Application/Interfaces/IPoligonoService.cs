
using FleetControl.Api.Application.DTOs.Poligonos;

namespace FleetControl.Api.Application.Interfaces;

public interface IPoligonoService
{
    Task<IEnumerable<PoligonoResponse>> ListarAsync();

    Task<PoligonoResponse?> BuscarPorIdAsync(long id);

    Task<PoligonoResponse> CriarAsync(
        CriarPoligonoRequest request);
}
