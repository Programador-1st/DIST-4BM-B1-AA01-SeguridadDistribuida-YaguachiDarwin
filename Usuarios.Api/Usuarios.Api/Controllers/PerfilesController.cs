using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.Api.Data;
using Usuarios.Api.DTOs;
using Usuarios.Api.Models;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/perfiles")]
public class PerfilesController : ControllerBase
{
    private readonly UsuariosDbContext _context;

    public PerfilesController(UsuariosDbContext context) => _context = context;

    
    [HttpPost]
    [AllowAnonymous] 
    public async Task<IActionResult> CrearPerfil([FromBody] CrearPerfilDto dto)
    {
        if (await _context.Perfiles.AnyAsync(p => p.UsuarioId == dto.Id))
            return Conflict(new { mensaje = "El perfil ya existe para este usuario." });

        var perfil = new Perfil
        {
            UsuarioId = dto.Id,
            Nombres = dto.Nombre,
            Apellidos = dto.Apellido,
            Correo = dto.Correo
        };

        _context.Perfiles.Add(perfil);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerMiPerfil), new { }, new { mensaje = "Perfil creado" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> ObtenerMiPerfil()
    {

        var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (usuarioId is null) return Unauthorized();

        var perfil = await _context.Perfiles.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (perfil is null) return NotFound(new { mensaje = "Perfil no encontrado." });

        return Ok(new PerfilResponseDto
        {
            UsuarioId = perfil.UsuarioId,
            Nombres = perfil.Nombres,
            Apellidos = perfil.Apellidos,
            Correo = perfil.Correo,
            Telefono = perfil.Telefono,
            Direccion = perfil.Direccion,
            FechaNacimiento = perfil.FechaNacimiento
        });
    }


    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarPerfilDto dto)
    {
        var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (usuarioId is null) return Unauthorized();

        var perfil = await _context.Perfiles.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (perfil is null) return NotFound(new { mensaje = "Perfil no encontrado." });

        perfil.Telefono = dto.Telefono ?? perfil.Telefono;
        perfil.Direccion = dto.Direccion ?? perfil.Direccion;
        perfil.FechaNacimiento = dto.FechaNacimiento ?? perfil.FechaNacimiento;
        perfil.FechaModificacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Perfil actualizado correctamente." });
    }
}