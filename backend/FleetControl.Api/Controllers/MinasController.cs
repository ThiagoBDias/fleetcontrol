
using FleetControl.Api.Application.DTOs.Minas;
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

    // GET: api/minas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MinaResponse>>> GetMinas()
    {
        var minas = await _context.Minas
            .AsNoTracking()
            .Select(m => new MinaResponse
            {
                Id = m.Id,
                EmpresaId = m.EmpresaId,
                Nome = m.Nome,
                Codigo = m.Codigo,
                Ativo = m.Ativo,
                CriadoEm = m.CriadoEm
            })
            .ToListAsync();

        return Ok(minas);
    }

    // GET: api/minas/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<MinaResponse>> GetMina(long id)
    {
        var mina = await _context.Minas
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new MinaResponse
            {
                Id = m.Id,
                EmpresaId = m.EmpresaId,
                Nome = m.Nome,
                Codigo = m.Codigo,
                Ativo = m.Ativo,
                CriadoEm = m.CriadoEm
            })
            .FirstOrDefaultAsync();

        if (mina is null)
            return NotFound();

        return Ok(mina);
    }

    // POST: api/minas
    [HttpPost]
    public async Task<ActionResult<MinaResponse>> CriarMina(
        CriarMinaRequest request)
    {
        var empresaExiste = await _context.Empresas
            .AnyAsync(e => e.Id == request.EmpresaId);

        if (!empresaExiste)
            return BadRequest("Empresa não encontrada.");

        var mina = new Mina
        {
            EmpresaId = request.EmpresaId,
            Nome = request.Nome.Trim(),
            Codigo = request.Codigo?.Trim(),
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        _context.Minas.Add(mina);

        await _context.SaveChangesAsync();

        var response = new MinaResponse
        {
            Id = mina.Id,
            EmpresaId = mina.EmpresaId,
            Nome = mina.Nome,
            Codigo = mina.Codigo,
            Ativo = mina.Ativo,
            CriadoEm = mina.CriadoEm
        };

        return CreatedAtAction(
            nameof(GetMina),
            new { id = mina.Id },
            response);
    }
}
