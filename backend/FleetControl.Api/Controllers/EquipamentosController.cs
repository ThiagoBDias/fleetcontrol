
using FleetControl.Api.Application.DTOs.Equipamentos;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipamentosController : ControllerBase
{
    private readonly IEquipamentoService _service;

    public EquipamentosController(IEquipamentoService service)
    {
        _service = service;
    }

    // GET: api/equipamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipamentoResponse>>> GetEquipamentos()
    {
        var equipamentos = await _service.ListarAsync();

        return Ok(equipamentos);
    }

    // GET: api/equipamentos/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EquipamentoResponse>> GetEquipamento(long id)
    {
        var equipamento = await _service.BuscarPorIdAsync(id);

        if (equipamento is null)
            return NotFound();

        return Ok(equipamento);
    }

    // POST: api/equipamentos
    [HttpPost]
    public async Task<ActionResult<EquipamentoResponse>> CriarEquipamento(
        CriarEquipamentoRequest request)
    {
        var equipamento = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetEquipamento),
            new { id = equipamento.Id },
            equipamento);
    }
}
