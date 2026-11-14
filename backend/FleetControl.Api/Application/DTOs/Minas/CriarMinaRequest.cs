
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.Minas;

public class CriarMinaRequest
{
    [Range(typeof(long), "1", "9223372036854775807")]
    public long EmpresaId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Codigo { get; set; }
}

