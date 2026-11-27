using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Data;

public class ApplicationDbContext : IdentityDbContext<AplicacionUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        
        builder.Entity<AplicacionUser>(b =>
        {
            b.ToTable("Usuarios");
            b.Property(u => u.Id).HasColumnName("Id");
            b.Property(u => u.UserName).HasColumnName("NombreUsuario").HasMaxLength(256);
            b.Property(u => u.NormalizedUserName).HasColumnName("NombreUsuarioNormalizado").HasMaxLength(256);
            b.Property(u => u.Email).HasColumnName("Correo").HasMaxLength(256);
            b.Property(u => u.NormalizedEmail).HasColumnName("CorreoNormalizado").HasMaxLength(256);
            b.Property(u => u.EmailConfirmed).HasColumnName("CorreoConfirmado");
            b.Property(u => u.PasswordHash).HasColumnName("HashContrasena");
            b.Property(u => u.SecurityStamp).HasColumnName("SecurityStamp");
            b.Property(u => u.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
            b.Property(u => u.PhoneNumber).HasColumnName("Telefono");
            b.Property(u => u.PhoneNumberConfirmed).HasColumnName("TelefonoConfirmado");
            b.Property(u => u.TwoFactorEnabled).HasColumnName("DosFactoresHabilitado");
            b.Property(u => u.LockoutEnd).HasColumnName("BloqueoFin");
            b.Property(u => u.LockoutEnabled).HasColumnName("BloqueoHabilitado");
            b.Property(u => u.AccessFailedCount).HasColumnName("IntentosFallidos");

         
            b.Property(u => u.FechaRegistro).HasColumnName("FechaRegistro");
            b.Property(u => u.NombreCompleto).HasColumnName("NombreCompleto").HasMaxLength(150);
            b.Property(u => u.EstadoCuenta).HasColumnName("EstadoCuenta");
        });

        builder.Entity<IdentityRole>(b =>
        {
            b.ToTable("Roles");
            b.Property(r => r.Id).HasColumnName("Id");
            b.Property(r => r.Name).HasColumnName("Nombre").HasMaxLength(256);
            b.Property(r => r.NormalizedName).HasColumnName("NombreNormalizado").HasMaxLength(256);
        });

        builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UsuariosClaims"); });

        builder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.ToTable("UsuariosLogins");
            b.Property(l => l.LoginProvider).HasColumnName("ProveedorLogin");
            b.Property(l => l.ProviderKey).HasColumnName("ClaveProveedor");
            b.Property(l => l.ProviderDisplayName).HasColumnName("NombreMostrarProveedor");
            b.Property(l => l.UserId).HasColumnName("UsuarioId");
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });

        builder.Entity<IdentityUserToken<string>>(b =>
        {
            b.ToTable("UsuariosTokens");
            b.Property(t => t.UserId).HasColumnName("UsuarioId");
            b.Property(t => t.LoginProvider).HasColumnName("ProveedorLogin");
            b.Property(t => t.Name).HasColumnName("Nombre");
            b.Property(t => t.Value).HasColumnName("Valor");
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        });

        builder.Entity<IdentityUserRole<string>>(b =>
        {
            b.ToTable("UsuariosRoles");
            b.Property(r => r.UserId).HasColumnName("UsuarioId");
            b.Property(r => r.RoleId).HasColumnName("RolId");
            b.HasKey(r => new { r.UserId, r.RoleId });
        });

        builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RolesClaims"); });
    }
}