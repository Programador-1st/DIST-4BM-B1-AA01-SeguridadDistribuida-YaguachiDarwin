using System.ComponentModel.DataAnnotations;

namespace Usuarios.Api.DTOs;


public class CrearPerfilDto
{
    [Required] public string Id { get; set; } = null!; 
    [Required] public string Nombre { get; set; } = null!;
    [Required] public string Apellido { get; set; } = null!;
    [Required, EmailAddress] public string Correo { get; set; } = null!;
}


public class PerfilResponseDto
{
    public string UsuarioId { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}


public class ActualizarPerfilDto
{
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}