namespace FleetControl.Api.Domain.Entities;

public class Mina
{
    public long Id { get; set; }

    public long EmpresaId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Codigo { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    // Relacionamento com Empresa
    public Empresa Empresa { get; set; } = null!;

    // Relacionamentos com entidades da Mina
    public ICollection<Operador> Operadores { get; set; } = new List<Operador>();

    public ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();

    public ICollection<Poligono> Poligonos { get; set; } = new List<Poligono>();

    public ICollection<Material> Materiais { get; set; } = new List<Material>();

    public ICollection<Litologia> Litologias { get; set; } = new List<Litologia>();

    public ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
}