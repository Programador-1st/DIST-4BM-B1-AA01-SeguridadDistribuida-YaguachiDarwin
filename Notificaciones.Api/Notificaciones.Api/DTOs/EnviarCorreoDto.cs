using System.ComponentModel.DataAnnotations;

namespace Notificaciones.Api.DTOs;

public class EnviarCorreoDto
{
    [Required]
    public string TipoPlantilla { get; set; } = null!; 

    [Required, EmailAddress]
    public string Destinatario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Enlace { get; set; } 
    public string? UsuarioId { get; set; }
}