using System.Net.Http.Json;

namespace Auth.Api.Services;

public interface INotificacionesClient
{
    Task<bool> EnviarBienvenidaAsync(string correo, string nombre, CancellationToken ct = default);
    Task<bool> EnviarRecuperacionAsync(string correo, string nombre, string enlace, CancellationToken ct = default);
}

public class NotificacionesClient : INotificacionesClient
{
    private readonly HttpClient _http;
    private readonly ILogger<NotificacionesClient> _logger;

    public NotificacionesClient(HttpClient http, ILogger<NotificacionesClient> logger)
    {
        _http = http; _logger = logger;
    }

    public async Task<bool> EnviarBienvenidaAsync(string correo, string nombre, CancellationToken ct = default)
        => await EnviarAsync("Bienvenida", correo, nombre, null, ct);

    public async Task<bool> EnviarRecuperacionAsync(string correo, string nombre, string enlace, CancellationToken ct = default)
        => await EnviarAsync("RecuperacionPassword", correo, nombre, enlace, ct);

    private async Task<bool> EnviarAsync(string tipo, string correo, string nombre, string? enlace, CancellationToken ct)
    {
        try
        {
            var payload = new { tipoPlantilla = tipo, destinatario = correo, nombre, enlace };
            var resp = await _http.PostAsJsonAsync("/api/notificaciones/enviar", payload, ct);
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al llamar Notificaciones.Api");
            return false;
        }
    }
}