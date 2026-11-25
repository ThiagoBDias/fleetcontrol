
using FleetControl.Api.Application.DTOs.Minas;
using FleetControl.Api.Application.Exceptions;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Application.Services;

public class MinaService : IMinaService
{
    private readonly FleetControlDbContext _context;

    public MinaService(FleetControlDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MinaResponse>> ListarAsync()
    {
        return await _context.Minas
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
    }

    public async Task<MinaResponse?> BuscarPorIdAsync(long id)
    {
        return await _context.Minas
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
    }

    public async Task<MinaResponse> CriarAsync(CriarMinaRequest request)
    {
        var empresaExiste = await _context.Empresas
            .AnyAsync(e => e.Id == request.EmpresaId);

        if (!empresaExiste)
        {
            throw new BusinessRuleException(
                "Empresa não encontrada.");
        }

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

        return new MinaResponse
        {
            Id = mina.Id,
            EmpresaId = mina.EmpresaId,
            Nome = mina.Nome,
            Codigo = mina.Codigo,
            Ativo = mina.Ativo,
            CriadoEm = mina.CriadoEm
        };
    }
}
