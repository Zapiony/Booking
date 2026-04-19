using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Servicios.DataAccess.Queries;
using Microservicio.Servicios.DataAccess.Repositories;
using Microservicio.Servicios.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Implementación de <see cref="IUnitOfWork"/> sobre un <see cref="DbContext"/> compartido.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    private IServicioRepository? _servicios;
    private ITipoServicioRepository? _tiposServicio;
    private IServicioQueryRepository? _consultasServicio;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IServicioRepository Servicios =>
        _servicios ??= new ServicioRepository(_context);

    public ITipoServicioRepository TiposServicio =>
        _tiposServicio ??= new TipoServicioRepository(_context);

    public IServicioQueryRepository ConsultasServicio =>
        _consultasServicio ??= new ServicioQueryRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
