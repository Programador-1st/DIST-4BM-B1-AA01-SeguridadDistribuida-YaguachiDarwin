using System.ComponentModel.DataAnnotations;
namespace Auth.Api.DTOs;

public class GoogleLoginDto
{
    [Required] public string IdToken { get; set; } = null!;
}