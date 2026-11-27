
using FleetControl.Api.Application.DTOs.Poligonos;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoligonosController : ControllerBase
{
    private readonly IPoligonoService _service;

    public PoligonosController(IPoligonoService service)
    {
        _service = service;
    }

    // GET: api/poligonos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PoligonoResponse>>> GetPoligonos()
    {
        var poligonos = await _service.ListarAsync();

        return Ok(poligonos);
    }

    // GET: api/poligonos/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PoligonoResponse>> GetPoligono(long id)
    {
        var poligono = await _service.BuscarPorIdAsync(id);

        if (poligono is null)
            return NotFound();

        return Ok(poligono);
    }

    // POST: api/poligonos
    [HttpPost]
    public async Task<ActionResult<PoligonoResponse>> CriarPoligono(
        CriarPoligonoRequest request)
    {
        var poligono = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetPoligono),
            new { id = poligono.Id },
            poligono);
    }
}
