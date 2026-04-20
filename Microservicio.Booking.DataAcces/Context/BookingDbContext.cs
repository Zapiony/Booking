using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Servicios.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options) { }

    // Autenticación
    public DbSet<UsuarioAppEntity> UsuariosApp => Set<UsuarioAppEntity>();
    public DbSet<RolEntity> Roles => Set<RolEntity>();
    public DbSet<UsuarioRolEntity> UsuariosRoles => Set<UsuarioRolEntity>();

    // Dominio
    public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
    public DbSet<TipoServicioEntity> TiposServicio => Set<TipoServicioEntity>();
    public DbSet<ServicioEntity> Servicios => Set<ServicioEntity>();
    public DbSet<FacturacionEntity> Facturaciones => Set<FacturacionEntity>();

    // Trazabilidad
    public DbSet<LogAuditoriaEntity> LogsAuditoria => Set<LogAuditoriaEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BookingDbContext).Assembly
        );
    }
}