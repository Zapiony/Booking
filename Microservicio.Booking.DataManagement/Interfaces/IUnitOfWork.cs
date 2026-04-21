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
    // Usuarios
    IUsuarioRepository UsuarioRepository { get; }
    IRolRepository RolRepository { get; }
    UsuarioQueryRepository UsuarioQueryRepository { get; }

    // Clientes
    IClienteRepository ClienteRepository { get; }
    ClienteQueryRepository ClienteQueryRepository { get; }

    // Servicios
    IServicioRepository ServicioRepository { get; }
    ITipoServicioRepository TipoServicioRepository { get; }
    ServicioQueryRepository ServicioQueryRepository { get; }

    // Facturación
    IFacturacionRepository FacturacionRepository { get; }
    IFacturacionQueryRepository FacturacionQueryRepository { get; }

    // Auditoría
    ILogAuditoriaRepository LogAuditoriaRepository { get; }
    ILogAuditoriaQueryRepository LogAuditoriaQueryRepository { get; }

    // Persistencia
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}