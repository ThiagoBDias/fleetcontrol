
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.ModelosEquipamento;

public class CriarModeloEquipamentoRequest
{
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;
}
