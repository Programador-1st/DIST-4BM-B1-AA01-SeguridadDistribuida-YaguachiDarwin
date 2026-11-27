using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Models;

public class AplicacionUser : IdentityUser
{
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public string? NombreCompleto { get; set; }
    public int EstadoCuenta { get; set; } = 1; 
}