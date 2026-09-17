using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MinasController : ControllerBase
{
    private readonly FleetControlDbContext _context;

    public MinasController(FleetControlDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mina>>> GetMinas()
    {
        var minas = await _context.Minas
            .AsNoTracking()
            .ToListAsync();

        return Ok(minas);
    }

    [HttpPost]
public async Task<ActionResult<Mina>> CriarMina(Mina mina)
{
    _context.Minas.Add(mina);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetMinas),
        new { id = mina.Id },
        mina
    );
}

}