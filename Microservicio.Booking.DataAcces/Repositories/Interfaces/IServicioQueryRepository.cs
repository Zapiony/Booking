using Microservicio.Booking.DataAccess.Common;

namespace Microservicio.Booking.DataAccess.Repositories.Interfaces;

// =============================================================================
// DTOs de proyección — contratos de solo lectura propios de la DAL.
// =============================================================================

/// <summary>
/// Proyección plana de un servicio (proveedor) para listados y búsquedas.
/// Incluye el nombre del tipo de servicio para evitar roundtrips adicionales.
/// </summary>
public sealed record ServicioResumenDto(
    Guid GuidServicio,
    string RazonSocial,
    string? NombreComercial,
    string TipoServicioNombre,
    string Estado
);

/// <summary>
/// Proyección detallada de un servicio para vistas de administración.
/// </summary>
public sealed record ServicioDetalleDto(
    Guid GuidServicio,
    string RazonSocial,
    string? NombreComercial,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string CorreoContacto,
    string? TelefonoContacto,
    string? Direccion,
    string? SitioWeb,
    string? LogoUrl,
    string Estado,
    bool EsEliminado,
    Guid GuidTipoServicio,
    string TipoServicioNombre,
    string? CreadoPorUsuario,
    DateTimeOffset FechaRegistroUtc,
    string? ModificadoPorUsuario,
    DateTimeOffset? FechaModificacionUtc
);

// =============================================================================
// Contrato de consultas
// =============================================================================

/// <summary>
/// Contrato de consultas de solo lectura para el dominio de servicios.
/// Aplica el lado Query del patrón CQRS liviano.
/// </summary>
public interface IServicioQueryRepository
{
    /// <summary>
    /// Retorna una página de servicios en formato resumen con el nombre del tipo.
    /// </summary>
    Task<PagedResult<ServicioResumenDto>> ListarServiciosAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna el detalle completo de un servicio por su GUID público.
    /// Retorna null si no existe o está eliminado lógicamente.
    /// </summary>
    Task<ServicioDetalleDto?> ObtenerDetalleAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Búsqueda de servicios por término parcial sobre razón social
    /// o nombre comercial.
    /// </summary>
    Task<PagedResult<ServicioResumenDto>> BuscarServiciosAsync(
        string termino,
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna todos los servicios activos de un tipo específico.
    /// </summary>
    Task<IReadOnlyList<ServicioResumenDto>> ListarServiciosPorTipoAsync(
        Guid tipoServicioGuid,
        CancellationToken cancellationToken = default);
}
