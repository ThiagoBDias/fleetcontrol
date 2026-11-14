
using FleetControl.Api.Application.DTOs.Empresas;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly FleetControlDbContext _context;

    public EmpresasController(FleetControlDbContext context)
    {
        _context = context;
    }

    // GET: api/empresas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpresaResponse>>> GetEmpresas()
    {
        var empresas = await _context.Empresas
            .AsNoTracking()
            .Select(e => new EmpresaResponse
            {
                Id = e.Id,
                Nome = e.Nome,
                Ativo = e.Ativo,
                CriadoEm = e.CriadoEm
            })
            .ToListAsync();

        return Ok(empresas);
    }

    // GET: api/empresas/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EmpresaResponse>> GetEmpresa(long id)
    {
        var empresa = await _context.Empresas
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EmpresaResponse
            {
                Id = e.Id,
                Nome = e.Nome,
                Ativo = e.Ativo,
                CriadoEm = e.CriadoEm
            })
            .FirstOrDefaultAsync();

        if (empresa is null)
            return NotFound();

        return Ok(empresa);
    }

    // POST: api/empresas
    [HttpPost]
    public async Task<ActionResult<EmpresaResponse>> CriarEmpresa(
        CriarEmpresaRequest request)
    {
        var empresa = new Empresa
        {
            Nome = request.Nome.Trim(),
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        _context.Empresas.Add(empresa);

        await _context.SaveChangesAsync();

        var response = new EmpresaResponse
        {
            Id = empresa.Id,
            Nome = empresa.Nome,
            Ativo = empresa.Ativo,
            CriadoEm = empresa.CriadoEm
        };

        return CreatedAtAction(
            nameof(GetEmpresa),
            new { id = empresa.Id },
            response);
    }
}
