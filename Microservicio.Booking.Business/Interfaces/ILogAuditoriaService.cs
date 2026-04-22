using Microservicio.Booking.Business.DTOs.LogAuditoria;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Interfaces;

/// <summary>
/// Contrato del servicio de negocio para los logs de auditoría.
/// Define los casos de uso para el registro y consulta de trazabilidad.
/// La API depende de esta interfaz, nunca de la implementación directa.
/// </summary>
public interface ILogAuditoriaService
{
    // -------------------------------------------------------------------------
    // Comandos (Escritura)
    // -------------------------------------------------------------------------
    Task<LogAuditoriaResponse> CrearAsync(
        CrearLogAuditoriaRequest request, 
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Consultas (Lectura)
    // -------------------------------------------------------------------------

    Task<DataPagedResult<LogAuditoriaResponse>> BuscarAsync(
        LogAuditoriaFiltroRequest request, 
        CancellationToken cancellationToken = default);
}
