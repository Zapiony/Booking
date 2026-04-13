using Microservicio.Usuarios.DataAccess.Entities;
using Microservicio.Usuarios.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Usuarios.DataAccess.Context;

/// <summary>
/// DbContext principal del microservicio de Gestión de Usuarios.
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

        // Aplica cada IEntityTypeConfiguration registrada en este ensamblado.
        // Al usar ApplyConfigurationsFromAssembly EF Core detecta automáticamente
        // cualquier clase que implemente IEntityTypeConfiguration<T>, por lo que
        // agregar una nueva Configuration no requiere modificar este método.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UsuarioDbContext).Assembly
        );
    }

    // -------------------------------------------------------------------------
    // Interceptor de concurrencia — manejo centralizado de conflictos ROWVERSION
    // -------------------------------------------------------------------------

    /// <summary>
    /// Sobrescribe SaveChangesAsync para capturar DbUpdateConcurrencyException
    /// y relanzarla con un mensaje claro hacia las capas superiores.
    /// La resolución del conflicto (last-write-wins vs. merge) es decisión
    /// de la capa de negocio, no de la DAL.
    /// </summary>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Re-lanza con contexto adicional para que la capa de negocio
            // pueda identificar qué entidad generó el conflicto.
            throw new DbUpdateConcurrencyException(
                $"Conflicto de concurrencia detectado en la entidad " +
                $"'{ex.Entries.FirstOrDefault()?.Metadata.Name ?? "desconocida"}'. " +
                $"El registro fue modificado por otro proceso. Vuelva a cargar y reintente.",
                ex
            );
        }
    }
}