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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
    {
        var empresas = await _context.Empresas
            .AsNoTracking()
            .ToListAsync();

        return Ok(empresas);
    }
    [HttpPost]
    public async Task<ActionResult<Empresa>> CriarEmpresa(Empresa empresa)
    {
        _context.Empresas.Add(empresa);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEmpresas),
            new { id = empresa.Id },
            empresa
        );
    }

}