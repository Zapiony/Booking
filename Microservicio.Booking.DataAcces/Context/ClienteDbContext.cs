using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Context;

/// <summary>
/// DbContext del microservicio de Clientes sobre PostgreSQL.
/// </summary>
public class ClienteDbContext : DbContext
{
    public ClienteDbContext(DbContextOptions<ClienteDbContext> options)
        : base(options)
    {
    }

    // DbSets

    ///Tabla booking.cliente
    public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();

    /// Tabla booking.usuario_app — solo lectura en este microservicio.
    public DbSet<UsuarioAppEntity> UsuariosApp => Set<UsuarioAppEntity>();

    // Configuración del modelo

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Registra automáticamente todas las IEntityTypeConfiguration
        // del ensamblado: ClienteConfiguration, UsuarioAppConfiguration, etc.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ClienteDbContext).Assembly
        );
    }

    // Intercepción de SaveChanges — Auditoría automática

    public override int SaveChanges()
    {
        AplicarAuditoria();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();
        return await base.SaveChangesAsync(cancellationToken);
    }

    // Auditoría

    private void AplicarAuditoria()
    {
        var ahora = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<ClienteEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.FechaRegistroUtc = ahora;
                    entry.Entity.EsEliminado = false;
                    entry.Entity.Estado = string.IsNullOrEmpty(entry.Entity.Estado)
                                                    ? "ACT"
                                                    : entry.Entity.Estado;
                    break;

                case EntityState.Modified:
                    entry.Entity.FechaModificacionUtc = ahora;
                    // Campos inmutables — nunca se actualizan
                    entry.Property(e => e.FechaRegistroUtc).IsModified = false;
                    entry.Property(e => e.CreadoPorUsuario).IsModified = false;
                    entry.Property(e => e.GuidCliente).IsModified = false;
                    entry.Property(e => e.IdUsuario).IsModified = false;
                    break;
            }
        }
    }
}