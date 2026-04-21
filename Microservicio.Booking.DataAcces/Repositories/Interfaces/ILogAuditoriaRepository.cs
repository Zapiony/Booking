using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Entities;

namespace Microservicio.Booking.DataAccess.Repositories.Interfaces;

/// <summary>
/// Contrato del repositorio de Logs de Auditoría.
/// </summary>
public interface ILogAuditoriaRepository
{
    // Lecturas simples

    /// <summary>
    /// Obtiene un log mediante su identificador numérico interno.
    /// </summary>
    Task<LogAuditoriaEntity?> ObtenerPorIdAsync(long idLog, CancellationToken cancellationToken = default);

    // Lecturas paginadas

    /// <summary>
    /// Obtiene una lista paginada de todos los logs registrados.
    /// </summary>
    Task<PagedResult<LogAuditoriaEntity>> ObtenerTodosPaginadoAsync(int paginaActual, int tamanioPagina, CancellationToken cancellationToken = default);

    // Escritura

    /// <summary>
    /// Registra un nuevo log de auditoría manualmente. 
    /// (Nota: La DB suele hacerlo por triggers, esto cubre casos aplicacionales).
    /// </summary>
    Task AgregarAsync(LogAuditoriaEntity log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aplica el borrado lógico a un registro de log de auditoría.
    /// </summary>
    void EliminarLogico(LogAuditoriaEntity log);
}
