namespace FleetControl.Api.Domain.Entities;

public class Equipamento
{
    public long Id { get; set; }

    public long MinaId { get; set; }

    public long ModeloId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string? Nome { get; set; }

    public string Status { get; set; } = "ATIVO";

    public bool PossuiBalanca { get; set; }

    public bool Bloqueado { get; set; }

    public DateTime CriadoEm { get; set; }

    // Relacionamentos principais
    public Mina Mina { get; set; } = null!;

    public ModeloEquipamento Modelo { get; set; } = null!;

    // Relacionamentos operacionais
    public ICollection<AlocacaoEquipamento> Alocacoes { get; set; } = new List<AlocacaoEquipamento>();

    public ICollection<AlocacaoOperador> AlocacoesOperadores { get; set; } = new List<AlocacaoOperador>();

    public ICollection<Viagem> Viagens { get; set; } = new List<Viagem>();

    public ICollection<EventoViagem> EventosViagem { get; set; } = new List<EventoViagem>();
}