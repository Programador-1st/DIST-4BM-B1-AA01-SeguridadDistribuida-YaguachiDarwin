using Microsoft.EntityFrameworkCore;
using Usuarios.Api.Models;

namespace Usuarios.Api.Data;

public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : base(options) { }

    public DbSet<Perfil> Perfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Perfil>(b =>
        {
            b.ToTable("Perfiles");
            b.HasKey(p => p.Id);
            b.Property(p => p.UsuarioId).IsRequired();
            b.Property(p => p.Nombres).HasMaxLength(100).IsRequired();
            b.Property(p => p.Apellidos).HasMaxLength(100).IsRequired();
            b.Property(p => p.Correo).HasMaxLength(256).IsRequired();
            b.Property(p => p.Telefono).HasMaxLength(20);
            b.Property(p => p.Direccion).HasMaxLength(255);
        });
    }
}