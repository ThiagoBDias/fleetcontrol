
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.Operadores;

public class CriarOperadorRequest
{
    [Range(1, long.MaxValue)]
    public long MinaId { get; set; }

    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Matricula { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Cartao { get; set; }
}
