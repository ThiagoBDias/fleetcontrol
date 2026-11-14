
using FleetControl.Api.Application.DTOs.ModelosEquipamento;
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

    // GET: api/modelosequipamento
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModeloEquipamentoResponse>>> GetModelosEquipamento()
    {
        var modelos = await _context.ModelosEquipamento
            .AsNoTracking()
            .Select(m => new ModeloEquipamentoResponse
            {
                Id = m.Id,
                Nome = m.Nome,
                Tipo = m.Tipo,
                Ativo = m.Ativo
            })
            .ToListAsync();

        return Ok(modelos);
    }

    // GET: api/modelosequipamento/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ModeloEquipamentoResponse>> GetModeloEquipamento(long id)
    {
        var modelo = await _context.ModelosEquipamento
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new ModeloEquipamentoResponse
            {
                Id = m.Id,
                Nome = m.Nome,
                Tipo = m.Tipo,
                Ativo = m.Ativo
            })
            .FirstOrDefaultAsync();

        if (modelo is null)
            return NotFound();

        return Ok(modelo);
    }

    // POST: api/modelosequipamento
    [HttpPost]
    public async Task<ActionResult<ModeloEquipamentoResponse>> CriarModelo(
        CriarModeloEquipamentoRequest request)
    {
        var nome = request.Nome.Trim();

        var modeloExiste = await _context.ModelosEquipamento
            .AnyAsync(m => m.Nome == nome);

        if (modeloExiste)
            return Conflict("Já existe um modelo com esse nome.");

        var modelo = new ModeloEquipamento
        {
            Nome = nome,
            Tipo = request.Tipo.Trim(),
            Ativo = true
        };

        _context.ModelosEquipamento.Add(modelo);

        await _context.SaveChangesAsync();

        var response = new ModeloEquipamentoResponse
        {
            Id = modelo.Id,
            Nome = modelo.Nome,
            Tipo = modelo.Tipo,
            Ativo = modelo.Ativo
        };

        return CreatedAtAction(
            nameof(GetModeloEquipamento),
            new { id = modelo.Id },
            response);
    }
}
