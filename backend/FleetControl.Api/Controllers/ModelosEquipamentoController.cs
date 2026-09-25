using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelosEquipamentoController : ControllerBase
{
    private readonly FleetControlDbContext _context;

    public ModelosEquipamentoController(FleetControlDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<ActionResult<IEnumerable<ModeloEquipamento>>> GetModelosEquipamento()
{
    var modelos = await _context.ModelosEquipamento
        .AsNoTracking()
        .ToListAsync();

    return Ok(modelos);
}

[HttpPost]
public async Task<ActionResult<ModeloEquipamento>> CriarModelo(ModeloEquipamento modelo)
{
    _context.ModelosEquipamento.Add(modelo);

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetModelosEquipamento),
        new { id = modelo.Id },
        modelo
    );
}
}
