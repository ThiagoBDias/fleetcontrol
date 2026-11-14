using NetTopologySuite.Geometries;

namespace FleetControl.Api.Domain.Entities;

public class Poligono
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public Polygon Geometria { get; set; } = null!;

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    // Relacionamentos
    public Mina Mina { get; set; } = null!;

    public ICollection<Locacao> LocacoesOrigem { get; set; } = new List<Locacao>();

    public ICollection<Locacao> LocacoesDestino { get; set; } = new List<Locacao>();

    public ICollection<EventoViagem> EventosViagem { get; set; } = new List<EventoViagem>();
}