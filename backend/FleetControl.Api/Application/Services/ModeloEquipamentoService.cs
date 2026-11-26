
using FleetControl.Api.Application.DTOs.ModelosEquipamento;
using FleetControl.Api.Application.Exceptions;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Application.Services;

public class ModeloEquipamentoService : IModeloEquipamentoService
{
    private readonly FleetControlDbContext _context;

    public ModeloEquipamentoService(FleetControlDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ModeloEquipamentoResponse>> ListarAsync()
    {
        return await _context.ModelosEquipamento
            .AsNoTracking()
            .Select(m => new ModeloEquipamentoResponse
            {
                Id = m.Id,
                Nome = m.Nome,
                Tipo = m.Tipo,
                Ativo = m.Ativo
            })
            .ToListAsync();
    }

    public async Task<ModeloEquipamentoResponse?> BuscarPorIdAsync(long id)
    {
        return await _context.ModelosEquipamento
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
    }

    public async Task<ModeloEquipamentoResponse> CriarAsync(
        CriarModeloEquipamentoRequest request)
    {
        var nome = request.Nome.Trim();

        var modeloExiste = await _context.ModelosEquipamento
            .AnyAsync(m => m.Nome == nome);

        if (modeloExiste)
        {
            throw new ConflictException(
                "Já existe um modelo com esse nome.");
        }

        var modelo = new ModeloEquipamento
        {
            Nome = nome,
            Tipo = request.Tipo.Trim(),
            Ativo = true
        };

        _context.ModelosEquipamento.Add(modelo);

        await _context.SaveChangesAsync();

        return new ModeloEquipamentoResponse
        {
            Id = modelo.Id,
            Nome = modelo.Nome,
            Tipo = modelo.Tipo,
            Ativo = modelo.Ativo
        };
    }
}
