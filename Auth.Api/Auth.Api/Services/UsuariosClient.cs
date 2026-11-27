using System.Net.Http.Json;

namespace Auth.Api.Services;

public interface IUsuariosClient
{
    Task<bool> CrearPerfilAsync(Guid usuarioId, string nombre, string apellido, string correo, CancellationToken ct = default);
}

public class UsuariosClient : IUsuariosClient
{
    private readonly HttpClient _http;
    private readonly ILogger<UsuariosClient> _logger;

    public UsuariosClient(HttpClient http, ILogger<UsuariosClient> logger)
    {
        _http = http; _logger = logger;
    }

    public async Task<bool> CrearPerfilAsync(Guid usuarioId, string nombre, string apellido, string correo, CancellationToken ct = default)
    {
        try
        {
            var payload = new { id = usuarioId, nombre, apellido, correo };
            var resp = await _http.PostAsJsonAsync("/api/perfiles", payload, ct);
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al llamar Usuarios.Api");
            return false;
        }
    }
}