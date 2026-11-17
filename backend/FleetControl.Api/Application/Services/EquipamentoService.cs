
using FleetControl.Api.Application.DTOs.Equipamentos;
using FleetControl.Api.Application.Exceptions;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Application.Services;

public class EquipamentoService : IEquipamentoService
{
    private readonly FleetControlDbContext _context;

    public EquipamentoService(FleetControlDbContext context)
    {
        _context = context;
    }

    // Listar equipamentos
    public async Task<IEnumerable<EquipamentoResponse>> ListarAsync()
    {
        return await _context.Equipamentos
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
    }

    // Buscar equipamento por ID
    public async Task<EquipamentoResponse?> BuscarPorIdAsync(long id)
    {
        return await _context.Equipamentos
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
    }

    // Cadastrar equipamento
    public async Task<EquipamentoResponse> CriarAsync(
        CriarEquipamentoRequest request)
    {
        var codigo = request.Codigo.Trim();

        // Validar código
        if (string.IsNullOrWhiteSpace(codigo))
        {
            throw new BusinessRuleException(
                "O código do equipamento é obrigatório.");
        }

        // Verificar existência da mina
        var minaExiste = await _context.Minas
            .AnyAsync(m => m.Id == request.MinaId);

        if (!minaExiste)
        {
            throw new BusinessRuleException(
                "Mina não encontrada.");
        }

        // Verificar existência do modelo
        var modeloExiste = await _context.ModelosEquipamento
            .AnyAsync(m => m.Id == request.ModeloId);

        if (!modeloExiste)
        {
            throw new BusinessRuleException(
                "Modelo de equipamento não encontrado.");
        }

        // Verificar duplicidade
        var equipamentoExiste = await _context.Equipamentos
            .AnyAsync(e =>
                e.MinaId == request.MinaId &&
                e.Codigo == codigo);

        if (equipamentoExiste)
        {
            throw new ConflictException(
                "Já existe um equipamento com esse código nesta mina.");
        }

        // Criar equipamento
        var equipamento = new Equipamento
        {
            MinaId = request.MinaId,
            ModeloId = request.ModeloId,
            Codigo = codigo,
            Nome = request.Nome,
            PossuiBalanca = request.PossuiBalanca,
            Status = "ATIVO",
            Bloqueado = false,
            CriadoEm = DateTime.UtcNow
        };

        _context.Equipamentos.Add(equipamento);

        await _context.SaveChangesAsync();

        // Retornar DTO
        return new EquipamentoResponse
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
    }
}
