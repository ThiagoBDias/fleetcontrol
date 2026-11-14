
using FleetControl.Api.Application.DTOs.Equipamentos;
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

    // GET: api/equipamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipamentoResponse>>> GetEquipamentos()
    {
        var equipamentos = await _context.Equipamentos
            .AsNoTracking()
            .Select(e => new EquipamentoResponse
            {
                Id = e.Id,
                MinaId = e.MinaId,
                ModeloId = e.ModeloId,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Status = e.Status,
                PossuiBalanca = e.PossuiBalanca,
                Bloqueado = e.Bloqueado,
                CriadoEm = e.CriadoEm
            })
            .ToListAsync();

        return Ok(equipamentos);
    }

    // GET: api/equipamentos/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<EquipamentoResponse>> GetEquipamento(long id)
    {
        var equipamento = await _context.Equipamentos
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EquipamentoResponse
            {
                Id = e.Id,
                MinaId = e.MinaId,
                ModeloId = e.ModeloId,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Status = e.Status,
                PossuiBalanca = e.PossuiBalanca,
                Bloqueado = e.Bloqueado,
                CriadoEm = e.CriadoEm
            })
            .FirstOrDefaultAsync();

        if (equipamento is null)
            return NotFound();

        return Ok(equipamento);
    }

    // POST: api/equipamentos
    [HttpPost]
    public async Task<ActionResult<EquipamentoResponse>> CriarEquipamento(
        CriarEquipamentoRequest request)
    {
        var equipamento = new Equipamento
        {
            MinaId = request.MinaId,
            ModeloId = request.ModeloId,
            Codigo = request.Codigo,
            Nome = request.Nome,
            PossuiBalanca = request.PossuiBalanca,
            Status = "ATIVO",
            Bloqueado = false,
            CriadoEm = DateTime.UtcNow
        };

        _context.Equipamentos.Add(equipamento);

        await _context.SaveChangesAsync();

        var response = new EquipamentoResponse
        {
            Id = equipamento.Id,
            MinaId = equipamento.MinaId,
            ModeloId = equipamento.ModeloId,
            Codigo = equipamento.Codigo,
            Nome = equipamento.Nome,
            Status = equipamento.Status,
            PossuiBalanca = equipamento.PossuiBalanca,
            Bloqueado = equipamento.Bloqueado,
            CriadoEm = equipamento.CriadoEm
        };

        return CreatedAtAction(
            nameof(GetEquipamento),
            new { id = equipamento.Id },
            response);
    }
}
