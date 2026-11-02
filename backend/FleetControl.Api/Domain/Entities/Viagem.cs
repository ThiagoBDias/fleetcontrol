namespace FleetControl.Api.Domain.Entities;

public class Viagem
{
    public Guid Codigo { get; set; }

    public long LocacaoId { get; set; }

    public long AlocacaoEquipamentoId { get; set; }

    public long OrigemPoligonoId { get; set; }

    public long DestinoPoligonoId { get; set; }

    public long? LitologiaId { get; set; }

    public long? EquipamentoCargaId { get; set; }

    public decimal Massa { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime? Fim { get; set; }

    public string Status { get; set; } = "EM_ANDAMENTO";

    public DateTime CriadoEm { get; set; }
}