
using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FleetControl.Api.Diagnostics;

public static class SpatialDiagnostics
{
    public static async Task ExecutarAsync(
        FleetControlDbContext context)
    {
        Console.WriteLine("=== TESTE ESPACIAL FLEETCONTROL ===");

        await using var transaction =
            await context.Database.BeginTransactionAsync();

        try
        {
            var geometryFactory = new GeometryFactory(
                new PrecisionModel(),
                4326);

            // Criar polígono de teste
            var geometria = geometryFactory.CreatePolygon(
                new Coordinate[]
                {
                    new(-47.000, -19.000),
                    new(-47.000, -19.001),
                    new(-47.001, -19.001),
                    new(-47.001, -19.000),
                    new(-47.000, -19.000)
                });

            var poligono = new Poligono
            {
                MinaId = 3,
                Nome = "TESTE-EF-POSTGIS",
                Tipo = "FRENTE",
                Geometria = geometria,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            };

            // Gravar no PostgreSQL
            context.Poligonos.Add(poligono);

            await context.SaveChangesAsync();

            Console.WriteLine($"Polígono criado: {poligono.Id}");

            // Ler novamente do banco
            var registro = await context.Poligonos
                .AsNoTracking()
                .SingleAsync(p => p.Id == poligono.Id);

            Console.WriteLine($"SRID: {registro.Geometria.SRID}");
            Console.WriteLine($"Geometria válida: {registro.Geometria.IsValid}");
            Console.WriteLine($"Tipo: {registro.Geometria.GeometryType}");

            // Simular coordenadas GPS
            var pontoInterno = geometryFactory.CreatePoint(
                new Coordinate(-47.0005, -19.0005));

            var pontoExterno = geometryFactory.CreatePoint(
                new Coordinate(-47.010, -19.010));

            // Executar consultas espaciais no PostGIS
            var dentro = await context.Poligonos
                .AnyAsync(p =>
                    p.Id == poligono.Id &&
                    p.Geometria.Contains(pontoInterno));

            var fora = await context.Poligonos
                .AnyAsync(p =>
                    p.Id == poligono.Id &&
                    p.Geometria.Contains(pontoExterno));

            Console.WriteLine($"Ponto interno: {dentro}");
            Console.WriteLine($"Ponto externo: {fora}");

            if (registro.Geometria.SRID != 4326 ||
                !registro.Geometria.IsValid ||
                !dentro ||
                fora)
            {
                throw new InvalidOperationException(
                    "Falha na validação espacial.");
            }

            Console.WriteLine("TESTE ESPACIAL APROVADO");
        }
        finally
        {
            await transaction.RollbackAsync();

            context.ChangeTracker.Clear();

            Console.WriteLine("Rollback realizado.");
        }
    }
}
