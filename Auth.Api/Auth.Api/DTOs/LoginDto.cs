using System.ComponentModel.DataAnnotations;
namespace Auth.Api.DTOs;

public class LoginDto
{
    [Required, EmailAddress] public string Correo { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}