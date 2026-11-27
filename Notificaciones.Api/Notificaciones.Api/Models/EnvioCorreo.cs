namespace Notificaciones.Api.Models;

public class EnvioCorreo
{
    public int Id { get; set; }
    public string Destinatario { get; set; } = null!;
    public string Asunto { get; set; } = null!;
    public string? Cuerpo { get; set; }
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "Pendiente"; 
    public string? MensajeError { get; set; }
    public string? UsuarioId { get; set; }
    public string? TipoPlantilla { get; set; }
}

public class Plantilla
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Asunto { get; set; } = null!;
    public string CuerpoHtml { get; set; } = null!;
}