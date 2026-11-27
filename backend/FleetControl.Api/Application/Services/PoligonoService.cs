
using FleetControl.Api.Application.DTOs.Poligonos;
using FleetControl.Api.Application.Exceptions;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace FleetControl.Api.Application.Services;

public class PoligonoService : IPoligonoService
{
    private readonly FleetControlDbContext _context;

    private readonly GeometryFactory _geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    public PoligonoService(FleetControlDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PoligonoResponse>> ListarAsync()
    {
        var poligonos = await _context.Poligonos
            .AsNoTracking()
            .ToListAsync();

        return poligonos.Select(MapearResponse).ToList();
    }

    public async Task<PoligonoResponse?> BuscarPorIdAsync(long id)
    {
        var poligono = await _context.Poligonos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return poligono is null
            ? null
            : MapearResponse(poligono);
    }

    public async Task<PoligonoResponse> CriarAsync(
        CriarPoligonoRequest request)
    {
        var nome = request.Nome?.Trim();
        var tipo = request.Tipo?.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessRuleException(
                "O nome do polígono é obrigatório.");

        if (nome.Length > 150)
            throw new BusinessRuleException(
                "O nome deve ter no máximo 150 caracteres.");

        if (tipo is not ("FRENTE" or "DESTINO" or "AREA_OPERACIONAL"))
            throw new BusinessRuleException(
                "Tipo de polígono inválido.");

        var minaExiste = await _context.Minas
            .AnyAsync(m => m.Id == request.MinaId);

        if (!minaExiste)
            throw new BusinessRuleException(
                "Mina não encontrada.");

        var pontos = request.Coordenadas;

        if (pontos is null || pontos.Length < 4)
            throw new BusinessRuleException(
                "O polígono precisa de pelo menos quatro coordenadas, incluindo o fechamento.");

        foreach (var ponto in pontos)
        {
            if (ponto is null || ponto.Length != 2)
                throw new BusinessRuleException(
                    "Cada ponto deve conter longitude e latitude.");

            var longitude = ponto[0];
            var latitude = ponto[1];

            if (!double.IsFinite(longitude) ||
                !double.IsFinite(latitude) ||
                longitude < -180 || longitude > 180 ||
                latitude < -90 || latitude > 90)
            {
                throw new BusinessRuleException(
                    "Longitude ou latitude inválida.");
            }
        }

        if (pontos[0][0] != pontos[^1][0] ||
            pontos[0][1] != pontos[^1][1])
        {
            throw new BusinessRuleException(
                "O polígono deve começar e terminar na mesma coordenada.");
        }

        var verticesDistintos = pontos
            .Take(pontos.Length - 1)
            .Select(p => (Longitude: p[0], Latitude: p[1]))
            .Distinct()
            .Count();

        if (verticesDistintos < 3)
            throw new BusinessRuleException(
                "O polígono precisa de pelo menos três vértices distintos.");

        var coordenadas = pontos
            .Select(p => new Coordinate(p[0], p[1]))
            .ToArray();

        var geometria = _geometryFactory.CreatePolygon(coordenadas);

        if (!geometria.IsValid || geometria.IsEmpty ||
            geometria.Area <= 0)
        {
            throw new BusinessRuleException(
                "A geometria do polígono é inválida.");
        }

        var poligono = new Poligono
        {
            MinaId = request.MinaId,
            Nome = nome,
            Tipo = tipo,
            Geometria = geometria,
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };

        _context.Poligonos.Add(poligono);

        await _context.SaveChangesAsync();

        return MapearResponse(poligono);
    }

    private static PoligonoResponse MapearResponse(Poligono poligono)
    {
        return new PoligonoResponse
        {
            Id = poligono.Id,
            MinaId = poligono.MinaId,
            Nome = poligono.Nome,
            Tipo = poligono.Tipo,
            Coordenadas = poligono.Geometria.ExteriorRing.Coordinates
                .Select(c => new[] { c.X, c.Y })
                .ToArray(),
            Ativo = poligono.Ativo,
            CriadoEm = poligono.CriadoEm
        };
    }
}
