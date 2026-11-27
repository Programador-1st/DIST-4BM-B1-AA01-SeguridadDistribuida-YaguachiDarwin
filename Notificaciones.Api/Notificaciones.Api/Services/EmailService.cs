using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Notificaciones.Api.Data;
using Notificaciones.Api.Models;

namespace Notificaciones.Api.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly NotificacionesDbContext _context;

    public EmailService(IConfiguration config, NotificacionesDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml, string tipo, string? usuarioId)
    {
        var log = new EnvioCorreo
        {
            Destinatario = destinatario,
            Asunto = asunto,
            TipoPlantilla = tipo,
            UsuarioId = usuarioId,
            Estado = "Pendiente",
            FechaEnvio = DateTime.UtcNow
        };

        try
        {
            var smtpHost = _config["SmtpSettings:Host"]!;
            var smtpPort = int.Parse(_config["SmtpSettings:Port"]!);
            var senderEmail = _config["SmtpSettings:SenderEmail"]!;
            var senderName = _config["SmtpSettings:SenderName"]!;
            var password = _config["SmtpSettings:Password"]!;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            mailMessage.To.Add(destinatario);

            await client.SendMailAsync(mailMessage);

            log.Estado = "Exitoso";
            log.Cuerpo = cuerpoHtml;
            _context.EnviosCorreo.Add(log);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            log.Estado = "Fallido";
            log.MensajeError = ex.Message;
            _context.EnviosCorreo.Add(log);
            await _context.SaveChangesAsync();
            return false;
        }
    }
}