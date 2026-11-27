using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notificaciones.Api.Data;
using Notificaciones.Api.DTOs;
using Notificaciones.Api.Services;

namespace Notificaciones.Api.Controllers;

[ApiController]
[Route("api/notificaciones")]
public class NotificacionesController : ControllerBase
{
    private readonly NotificacionesDbContext _context;
    private readonly EmailService _emailService;

    public NotificacionesController(NotificacionesDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] EnviarCorreoDto dto)
    {
        
        var plantilla = await _context.Plantillas.FirstOrDefaultAsync(p => p.Nombre == dto.TipoPlantilla);
        if (plantilla == null)
            return NotFound(new { mensaje = "Plantilla no encontrada." });

        var cuerpoHtml = plantilla.CuerpoHtml
            .Replace("{{nombre}}", dto.Nombre)
            .Replace("{{enlace}}", dto.Enlace ?? "");

        var exito = await _emailService.EnviarCorreoAsync(dto.Destinatario, plantilla.Asunto, cuerpoHtml, dto.TipoPlantilla, dto.UsuarioId);

        if (!exito)
            return StatusCode(500, new { mensaje = "Error al enviar el correo. Revise el log." });

        return Ok(new { mensaje = "Correo enviado exitosamente." });
    }
}