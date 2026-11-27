using System.ComponentModel.DataAnnotations;
namespace Auth.Api.DTOs;

public class RecuperarPasswordDto
{
    [Required, EmailAddress] public string Correo { get; set; } = null!;
}