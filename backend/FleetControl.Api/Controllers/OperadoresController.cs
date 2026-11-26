
using FleetControl.Api.Application.DTOs.Operadores;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperadoresController : ControllerBase
{
    private readonly IOperadorService _service;

    public OperadoresController(IOperadorService service)
    {
        _service = service;
    }

    // GET: api/operadores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperadorResponse>>> GetOperadores()
    {
        var operadores = await _service.ListarAsync();

        return Ok(operadores);
    }

    // GET: api/operadores/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<OperadorResponse>> GetOperador(long id)
    {
        var operador = await _service.BuscarPorIdAsync(id);

        if (operador is null)
            return NotFound();

        return Ok(operador);
    }

    // POST: api/operadores
    [HttpPost]
    public async Task<ActionResult<OperadorResponse>> CriarOperador(
        CriarOperadorRequest request)
    {
        var operador = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetOperador),
            new { id = operador.Id },
            operador);
    }
}

