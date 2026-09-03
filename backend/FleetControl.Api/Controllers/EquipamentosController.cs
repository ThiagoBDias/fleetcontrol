using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipamentosController : ControllerBase
{
    private readonly FleetControlDbContext _context;

    public EquipamentosController(FleetControlDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipamento>>> GetEquipamentos()
    {
        var equipamentos = await _context.Equipamentos
            .AsNoTracking()
            .ToListAsync();

        return Ok(equipamentos);
    }
}