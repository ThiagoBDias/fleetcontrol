
using System.ComponentModel.DataAnnotations;

namespace FleetControl.Api.Application.DTOs.Empresas;

public class CriarEmpresaRequest
{
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;
}
