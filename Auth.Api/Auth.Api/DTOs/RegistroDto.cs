using System.ComponentModel.DataAnnotations;
namespace Auth.Api.DTOs;

public class RegistroDto
{
    [Required] public string NombreCompleto { get; set; } = null!;
    [Required] public string Nombre { get; set; } = null!;
    [Required] public string Apellido { get; set; } = null!;
    [Required, EmailAddress] public string Correo { get; set; } = null!;
    [Required, MinLength(8)] public string Password { get; set; } = null!;
    [Required, Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")] public string ConfirmarPassword { get; set; } = null!;
}