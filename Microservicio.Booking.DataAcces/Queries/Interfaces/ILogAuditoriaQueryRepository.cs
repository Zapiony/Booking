using Microservicio.Booking.DataAccess.Common;

namespace Microservicio.Booking.DataAccess.Queries.Interfaces;

// =============================================================================
// DTOs de proyección
// =============================================================================

/// <summary>
/// Proyección de un log de auditoría resumido.
/// </summary>
public sealed record LogAuditoriaResumenDto(
    long IdLog,
    string TablaAfectada,
    string Operacion,
    string? CreadoPorUsuario,
    DateTimeOffset FechaUtc
);

// =============================================================================
// Contrato de consultas
// =============================================================================

/// <summary>
/// Contrato de consultas CQRS para los logs de auditoría.
/// </summary>
public interface ILogAuditoriaQueryRepository
{
    /// <summary>
    /// Lista los logs más recientes, filtrando opcionalmente por nombre de tabla.
    /// </summary>
    Task<PagedResult<LogAuditoriaResumenDto>> ListarLogsAsync(string? tablaAfectada, int paginaActual, int tamanoPagina, CancellationToken cancellationToken = default);
}
