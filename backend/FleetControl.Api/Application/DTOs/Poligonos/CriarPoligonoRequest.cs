
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.Poligonos;

public class CriarPoligonoRequest
{
    [Range(1, long.MaxValue)]
    public long MinaId { get; set; }

    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    public double[][] Coordenadas { get; set; } = [];
}
