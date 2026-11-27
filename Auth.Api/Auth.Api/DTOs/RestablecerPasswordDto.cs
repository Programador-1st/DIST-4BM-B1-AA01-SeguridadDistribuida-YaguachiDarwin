using System.ComponentModel.DataAnnotations;
namespace Auth.Api.DTOs;

public class RestablecerPasswordDto
{
    [Required, EmailAddress] public string Correo { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
    [Required, MinLength(8)] public string NuevaPassword { get; set; } = null!;
}