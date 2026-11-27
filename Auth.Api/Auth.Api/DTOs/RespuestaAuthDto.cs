namespace Auth.Api.DTOs;

public class RespuestaAuthDto
{
    public string Token { get; set; } = null!;
    public DateTime Expira { get; set; }
    public string UsuarioId { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}