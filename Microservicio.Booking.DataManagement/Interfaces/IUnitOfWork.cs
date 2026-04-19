using Microservicio.Servicios.DataAccess.Queries;
using Microservicio.Servicios.DataAccess.Repositories.Interfaces;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Orquesta repositorios de persistencia y consultas del dominio Servicios / TipoServicio.
/// </summary>
public interface IUnitOfWork
{
    IServicioRepository Servicios { get; }
    ITipoServicioRepository TiposServicio { get; }
    IServicioQueryRepository ConsultasServicio { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
