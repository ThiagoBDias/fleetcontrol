namespace FleetControl.Api.Domain.Entities;

public class Litologia
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public long MaterialId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Tipo { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    // Relacionamentos
    public Mina Mina { get; set; } = null!;

    public Material Material { get; set; } = null!;

    public ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();
}