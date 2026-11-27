using Auth.Api.DTOs;
using Auth.Api.Models;
using Auth.Api.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AplicacionUser> _userManager;
    private readonly SignInManager<AplicacionUser> _signInManager;
    private readonly IJwtService _jwt;
    private readonly IUsuariosClient _usuarios;
    private readonly INotificacionesClient _notificaciones;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<AplicacionUser> userManager,
        SignInManager<AplicacionUser> signInManager,
        IJwtService jwt,
        IUsuariosClient usuarios,
        INotificacionesClient notificaciones,
        IConfiguration config,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwt = jwt;
        _usuarios = usuarios;
        _notificaciones = notificaciones;
        _config = config;
        _logger = logger;
    }

    [HttpPost("registro")]
    [AllowAnonymous]
    public async Task<IActionResult> Registro([FromBody] RegistroDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existente = await _userManager.FindByEmailAsync(dto.Correo);
        if (existente is not null) return Conflict(new { mensaje = "El correo ya está registrado." });

        var usuario = new AplicacionUser
        {
            UserName = dto.Correo,
            Email = dto.Correo,
            NombreCompleto = dto.NombreCompleto,
            EmailConfirmed = true,
            EstadoCuenta = 1,
            LockoutEnabled = true
        };

        var resultado = await _userManager.CreateAsync(usuario, dto.Password);
        if (!resultado.Succeeded) return BadRequest(new { errores = resultado.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(usuario, "Cliente");

        
        var perfilOk = await _usuarios.CrearPerfilAsync(Guid.Parse(usuario.Id), dto.Nombre, dto.Apellido, dto.Correo);
        if (!perfilOk)
        {
            await _userManager.DeleteAsync(usuario);
            return StatusCode(503, new { mensaje = "No se pudo crear el perfil. Intente más tarde." });
        }

    
        _ = Task.Run(async () => {
            try { await _notificaciones.EnviarBienvenidaAsync(dto.Correo, dto.NombreCompleto); }
            catch {  }
        });

        return Created(string.Empty, new { mensaje = "Registro exitoso.", usuarioId = usuario.Id });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Correo);
        if (usuario is null) return Unauthorized(new { mensaje = "Credenciales inválidas." });
        if (usuario.EstadoCuenta != 1) return Unauthorized(new { mensaje = "La cuenta no está activa." });

        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, dto.Password, lockoutOnFailure: true);
        if (!resultado.Succeeded) return Unauthorized(new { mensaje = "Credenciales inválidas o cuenta bloqueada." });

        var roles = await _userManager.GetRolesAsync(usuario);
        var token = _jwt.GenerarToken(usuario, roles);

        return Ok(new RespuestaAuthDto
        {
            Token = token,
            Expira = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpiraMinutos")),
            UsuarioId = usuario.Id,
            Correo = usuario.Email!,
            Roles = roles.ToList()
        });
    }

    [HttpPost("google-login")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var clientId = _config["Google:ClientId"];
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings { Audience = new[] { clientId } });
        }
        catch { return Unauthorized(new { mensaje = "Token de Google inválido." }); }

        if (payload.EmailVerified != true) return Unauthorized(new { mensaje = "El correo no está verificado." });

        var usuario = await _userManager.FindByLoginAsync("Google", payload.Subject);
        if (usuario is null)
        {
            usuario = await _userManager.FindByEmailAsync(payload.Email);
            if (usuario is null)
            {
                usuario = new AplicacionUser { UserName = payload.Email, Email = payload.Email, NombreCompleto = payload.Name, EmailConfirmed = true, EstadoCuenta = 1 };
                await _userManager.CreateAsync(usuario);
                await _userManager.AddToRoleAsync(usuario, "Cliente");
                await _usuarios.CrearPerfilAsync(Guid.Parse(usuario.Id), payload.GivenName, payload.FamilyName, payload.Email);
            }
            await _userManager.AddLoginAsync(usuario, new UserLoginInfo("Google", payload.Subject, "Google"));
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        var token = _jwt.GenerarToken(usuario, roles);

        return Ok(new RespuestaAuthDto { Token = token, Expira = DateTime.UtcNow.AddMinutes(60), UsuarioId = usuario.Id, Correo = usuario.Email!, Roles = roles.ToList() });
    }

    [HttpPost("recuperar-password")]
    [AllowAnonymous]
    public async Task<IActionResult> RecuperarPassword([FromBody] RecuperarPasswordDto dto)
    {
        var respuesta = new { mensaje = "Si el correo existe, recibirás un enlace." };
        var usuario = await _userManager.FindByEmailAsync(dto.Correo);
        if (usuario is null) return Ok(respuesta);

        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

        
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var enlace = $"{baseUrl}/reset.html?correo={Uri.EscapeDataString(dto.Correo)}&token={Uri.EscapeDataString(token)}";

        try
        {
            await _notificaciones.EnviarRecuperacionAsync(dto.Correo, usuario.NombreCompleto ?? "", enlace);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar correo: {ex.Message}");
        }

        return Ok(respuesta);
    }


    [HttpPost("restablecer-password")]
    [AllowAnonymous]
    public async Task<IActionResult> RestablecerPassword([FromBody] RestablecerPasswordDto dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Correo);
        if (usuario is null) return BadRequest(new { mensaje = "Solicitud inválida." });

        var resultado = await _userManager.ResetPasswordAsync(usuario, dto.Token, dto.NuevaPassword);
        if (!resultado.Succeeded) return BadRequest(new { errores = resultado.Errors.Select(e => e.Description) });

        return Ok(new { mensaje = "Contraseña actualizada." });
    }
}