
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.Equipamentos;

public class CriarEquipamentoRequest
{
    [Range(1, long.MaxValue)]
    public long MinaId { get; set; }

    [Range(1, long.MaxValue)]
    public long ModeloId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Nome { get; set; }

    public bool PossuiBalanca { get; set; }
}
