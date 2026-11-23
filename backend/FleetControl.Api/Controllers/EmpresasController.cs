
using FleetControl.Api.Application.DTOs.Empresas;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _service;

    public EmpresasController(IEmpresaService service)
    {
        _service = service;
    }

    // GET: api/empresas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpresaResponse>>> GetEmpresas()
    {
        var empresas = await _service.ListarAsync();

        return Ok(empresas);
    }

    // GET: api/empresas/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EmpresaResponse>> GetEmpresa(long id)
    {
        var empresa = await _service.BuscarPorIdAsync(id);

        if (empresa is null)
            return NotFound();

        return Ok(empresa);
    }

    // POST: api/empresas
    [HttpPost]
    public async Task<ActionResult<EmpresaResponse>> CriarEmpresa(
        CriarEmpresaRequest request)
    {
        var empresa = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetEmpresa),
            new { id = empresa.Id },
            empresa);
    }
}
