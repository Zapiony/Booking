using Microservicio.Booking.DataAccess.Queries;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Unidad de trabajo: centraliza repositorios y el guardado transaccional.
/// La capa de negocio nunca llama SaveChanges directamente;
/// siempre lo hace a través de este contrato.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository UsuarioRepository { get; }
    IRolRepository RolRepository { get; }
    UsuarioQueryRepository UsuarioQueryRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}