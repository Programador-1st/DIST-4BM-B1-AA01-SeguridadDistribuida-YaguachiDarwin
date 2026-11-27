using Microsoft.EntityFrameworkCore;
using Notificaciones.Api.Models;

namespace Notificaciones.Api.Data;

public class NotificacionesDbContext : DbContext
{
    public NotificacionesDbContext(DbContextOptions<NotificacionesDbContext> options) : base(options) { }

    public DbSet<EnvioCorreo> EnviosCorreo { get; set; }
    public DbSet<Plantilla> Plantillas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnvioCorreo>(b => {
            b.ToTable("EnviosCorreo");
            b.HasKey(e => e.Id);
        });
        modelBuilder.Entity<Plantilla>(b => {
            b.ToTable("Plantillas");
            b.HasKey(p => p.Id);
        });
    }
}