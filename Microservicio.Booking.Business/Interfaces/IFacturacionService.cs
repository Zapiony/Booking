using Microservicio.Booking.Business.DTOs.Facturacion;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Interfaces;

/// <summary>
/// Contrato del servicio de negocio para facturación.
/// Define los casos de uso que expone la capa de negocio a la API.
/// La API depende de esta interfaz, nunca de la implementación directa.
/// </summary>
public interface IFacturacionService
{
    // -------------------------------------------------------------------------
    // Comandos (Escritura)
    // -------------------------------------------------------------------------
    Task<FacturacionResponse> CrearAsync(
        CrearFacturacionRequest request, 
        CancellationToken cancellationToken = default);

    Task<FacturacionResponse> ActualizarAsync(
        ActualizarFacturacionRequest request, 
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Consultas (Lectura)
    // -------------------------------------------------------------------------

    Task<FacturacionResponse> ObtenerPorGuidAsync(
        Guid guidFactura, 
        CancellationToken cancellationToken = default);

    Task<FacturacionResponse> ObtenerPorNumeroAsync(
        string numeroFactura, 
        CancellationToken cancellationToken = default);

    Task<DataPagedResult<FacturacionResponse>> BuscarAsync(
        FacturacionFiltroRequest request, 
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Operaciones de Estado
    // -------------------------------------------------------------------------

    Task EliminarLogicoAsync(
        Guid guidFactura, 
        string usuarioModificador, 
        CancellationToken cancellationToken = default);
}
