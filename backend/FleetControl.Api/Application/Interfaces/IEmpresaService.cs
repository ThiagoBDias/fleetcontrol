
using FleetControl.Api.Application.DTOs.Empresas;

namespace FleetControl.Api.Application.Interfaces;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaResponse>> ListarAsync();

    Task<EmpresaResponse?> BuscarPorIdAsync(long id);

    Task<EmpresaResponse> CriarAsync(CriarEmpresaRequest request);
}
