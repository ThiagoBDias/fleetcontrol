
using FleetControl.Api.Application.DTOs.Empresas;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Application.Services;

public class EmpresaService : IEmpresaService
{
    private readonly FleetControlDbContext _context;

    public EmpresaService(FleetControlDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmpresaResponse>> ListarAsync()
    {
        return await _context.Empresas
            .AsNoTracking()
            .Select(e => new EmpresaResponse
            {
                Id = e.Id,
                Nome = e.Nome,
                Ativo = e.Ativo,
                CriadoEm = e.CriadoEm
            })
            .ToListAsync();
    }

    public async Task<EmpresaResponse?> BuscarPorIdAsync(long id)
    {
        return await _context.Empresas
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
    }

    public async Task<EmpresaResponse> CriarAsync(
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

        return new EmpresaResponse
        {
            Id = empresa.Id,
            Nome = empresa.Nome,
            Ativo = empresa.Ativo,
            CriadoEm = empresa.CriadoEm
        };
    }
}
