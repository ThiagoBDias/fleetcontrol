
using FleetControl.Api.Application.DTOs.Minas;
using FleetControl.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MinasController : ControllerBase
{
    private readonly IMinaService _service;

    public MinasController(IMinaService service)
    {
        _service = service;
    }

    // GET: api/minas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MinaResponse>>> GetMinas()
    {
        var minas = await _service.ListarAsync();

        return Ok(minas);
    }

    // GET: api/minas/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<MinaResponse>> GetMina(long id)
    {
        var mina = await _service.BuscarPorIdAsync(id);

        if (mina is null)
            return NotFound();

        return Ok(mina);
    }

    // POST: api/minas
    [HttpPost]
    public async Task<ActionResult<MinaResponse>> CriarMina(
        CriarMinaRequest request)
    {
        var mina = await _service.CriarAsync(request);

        return CreatedAtAction(
            nameof(GetMina),
            new { id = mina.Id },
            mina);
    }
}
