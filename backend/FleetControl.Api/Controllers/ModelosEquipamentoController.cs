
using FleetControl.Api.Application.DTOs.ModelosEquipamento;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelosEquipamentoController : ControllerBase
{
    private readonly IModeloEquipamentoService _service;

    public ModelosEquipamentoController(
        IModeloEquipamentoService service)
    {
        _service = service;
    }

    // GET: api/modelosequipamento
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModeloEquipamentoResponse>>>
        GetModelosEquipamento()
    {
        var modelos = await _service.ListarAsync();

        return Ok(modelos);
    }

    // GET: api/modelosequipamento/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ModeloEquipamentoResponse>>
        GetModeloEquipamento(long id)
    {
        var modelo = await _service.BuscarPorIdAsync(id);

        if (modelo is null)
            return NotFound();

        return Ok(modelo);
    }

    // POST: api/modelosequipamento
    [HttpPost]
    public async Task<ActionResult<ModeloEquipamentoResponse>>
        CriarModelo(CriarModeloEquipamentoRequest request)
    {
        var modelo = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetModeloEquipamento),
            new { id = modelo.Id },
            modelo);
    }
}
