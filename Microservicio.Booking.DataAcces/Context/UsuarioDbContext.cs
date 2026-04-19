using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Context;

/// <summary>
/// DbContext principal del microservicio de Gestión de Usuarios.
/// Motor: PostgreSQL (Npgsql — EF Core).
/// Expone los DbSet de las tres entidades del dominio y aplica
/// todas las configuraciones Fluent API al construir el modelo.
/// </summary>
public class UsuarioDbContext : DbContext
{
    // -------------------------------------------------------------------------
    // Constructor — recibe opciones desde inyección de dependencias
    // -------------------------------------------------------------------------

    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options)
        : base(options)
    {
    }

    // -------------------------------------------------------------------------
    // DbSets — una propiedad por cada tabla del dominio de usuarios
    // -------------------------------------------------------------------------

    /// <summary>
    /// Tabla booking.usuario_app — credenciales y estado del usuario.
    /// </summary>
    public DbSet<UsuarioAppEntity> Usuarios { get; set; }

    /// <summary>
    /// Tabla booking.rol — catálogo de roles del sistema.
    /// </summary>
    public DbSet<RolEntity> Roles { get; set; }

    /// <summary>
    /// Tabla puente booking.usuarios_roles — asignación N:M usuario-rol.
    /// </summary>
    public DbSet<UsuarioRolEntity> UsuariosRoles { get; set; }

    // -------------------------------------------------------------------------
    // Construcción del modelo — aplica las configurations por entidad
    // -------------------------------------------------------------------------

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las clases que implementen
        // IEntityTypeConfiguration<T> dentro de este ensamblado.
        // Agregar una nueva entidad solo requiere crear su Configuration;
        // no es necesario modificar este método.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UsuarioDbContext).Assembly
        );
    }
}