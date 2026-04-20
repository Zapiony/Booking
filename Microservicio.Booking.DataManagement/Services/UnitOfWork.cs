using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataAccess.Queries;
using Microservicio.Booking.DataAccess.Repositories;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Implementación de IUnitOfWork.
/// Centraliza los repositorios de la capa 1 y el SaveChangesAsync
/// para que todas las operaciones de una misma solicitud compartan
/// el mismo DbContext y sean atómicas.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly UsuarioDbContext _context;

    public IUsuarioRepository UsuarioRepository { get; }
    public IRolRepository RolRepository { get; }
    public UsuarioQueryRepository UsuarioQueryRepository { get; }

    public UnitOfWork(UsuarioDbContext context)
    {
        _context = context;
        UsuarioRepository = new UsuarioRepository(_context);
        RolRepository = new RolRepository(_context);
        UsuarioQueryRepository = new UsuarioQueryRepository(_context);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}