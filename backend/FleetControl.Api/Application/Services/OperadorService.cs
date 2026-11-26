
using FleetControl.Api.Application.DTOs.Operadores;
using FleetControl.Api.Application.Exceptions;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Application.Services;

public class OperadorService : IOperadorService
{
    private readonly FleetControlDbContext _context;

    public OperadorService(FleetControlDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OperadorResponse>> ListarAsync()
    {
        return await _context.Operadores
            .AsNoTracking()
            .Select(o => new OperadorResponse
            {
                Id = o.Id,
                MinaId = o.MinaId,
                Nome = o.Nome,
                Matricula = o.Matricula,
                Cartao = o.Cartao,
                Status = o.Status,
                CriadoEm = o.CriadoEm
            })
            .ToListAsync();
    }

    public async Task<OperadorResponse?> BuscarPorIdAsync(long id)
    {
        return await _context.Operadores
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OperadorResponse
            {
                Id = o.Id,
                MinaId = o.MinaId,
                Nome = o.Nome,
                Matricula = o.Matricula,
                Cartao = o.Cartao,
                Status = o.Status,
                CriadoEm = o.CriadoEm
            })
            .FirstOrDefaultAsync();
    }

    public async Task<OperadorResponse> CriarAsync(
        CriarOperadorRequest request)
    {
        var nome = request.Nome?.Trim();
        var matricula = request.Matricula?.Trim();
        var cartao = request.Cartao?.Trim();

        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessRuleException(
                "O nome do operador é obrigatório.");

        if (string.IsNullOrWhiteSpace(matricula))
            throw new BusinessRuleException(
                "A matrícula do operador é obrigatória.");

        var minaExiste = await _context.Minas
            .AnyAsync(m => m.Id == request.MinaId);

        if (!minaExiste)
            throw new BusinessRuleException(
                "Mina não encontrada.");

        var matriculaExiste = await _context.Operadores
            .AnyAsync(o =>
                o.MinaId == request.MinaId &&
                o.Matricula == matricula);

        if (matriculaExiste)
            throw new ConflictException(
                "Já existe um operador com essa matrícula nesta mina.");

        var operador = new Operador
        {
            MinaId = request.MinaId,
            Nome = nome,
            Matricula = matricula,
            Cartao = string.IsNullOrWhiteSpace(cartao) ? null : cartao,
            Status = "ATIVO",
            CriadoEm = DateTime.UtcNow
        };

        _context.Operadores.Add(operador);
        await _context.SaveChangesAsync();

        return new OperadorResponse
        {
            Id = operador.Id,
            MinaId = operador.MinaId,
            Nome = operador.Nome,
            Matricula = operador.Matricula,
            Cartao = operador.Cartao,
            Status = operador.Status,
            CriadoEm = operador.CriadoEm
        };
    }
}
