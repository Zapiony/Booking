using Microservicio.Booking.DataAccess.Common;

namespace Microservicio.Booking.DataAccess.Queries.Interfaces;

// =============================================================================
// DTOs de proyección — contratos de solo lectura propios de la DAL.
// =============================================================================

/// <summary>
/// Proyección plana de una facturación para listados y búsquedas rápidas.
/// Incluye los totales y estado sin exponer información interna de la base.
/// </summary>
public sealed record FacturacionResumenDto(
    Guid GuidFacturacion,
    string NumeroFactura,
    decimal Total,
    string Estado,
    DateTimeOffset FechaRegistroUtc
);

// =============================================================================
// Contrato de consultas
// =============================================================================

/// <summary>
/// Contrato de consultas de solo lectura para el dominio de facturación.
/// Aplica el lado Query del patrón CQRS liviano.
/// </summary>
public interface IFacturacionQueryRepository
{
    /// <summary>
    /// Retorna una página de facturaciones en formato resumen.
    /// </summary>
    Task<PagedResult<FacturacionResumenDto>> ListarFacturacionesAsync(int paginaActual, int tamanoPagina, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna el resumen de una facturación específica por su GUID público.
    /// Retorna null si no existe o está eliminada lógicamente.
    /// </summary>
    Task<FacturacionResumenDto?> ObtenerResumenPorGuidAsync(Guid guidFacturacion, CancellationToken cancellationToken = default);
}
