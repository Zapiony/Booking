using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Common;

namespace Microservicio.Booking.DataAccess.Repositories.Interfaces;

/// <summary>
/// Contrato del repositorio de facturaciones.
/// Ningún método llama SaveChanges directamente —
/// esa responsabilidad recae en la Unidad de Trabajo (UoW) de la capa superior.
/// </summary>
public interface IFacturacionRepository
{
    // Lecturas simples

    /// <summary>
    /// Obtiene una facturación mediante su identificador interno.
    /// </summary>
    Task<FacturacionEntity?> ObtenerPorIdAsync(int idFactura, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una facturación mediante su identificador público (Guid).
    /// </summary>
    Task<FacturacionEntity?> ObtenerPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una facturación mediante su número único.
    /// </summary>
    Task<FacturacionEntity?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una facturación por Guid permitiendo el tracking de EF Core para operaciones de actualización.
    /// </summary>
    Task<FacturacionEntity?> ObtenerParaActualizarAsync(Guid guidFactura, CancellationToken cancellationToken = default);

    // Lecturas paginadas

    /// <summary>
    /// Obtiene una lista paginada con todas las facturaciones vigentes.
    /// </summary>
    Task<PagedResult<FacturacionEntity>> ObtenerTodosPaginadoAsync(int paginaActual, int tamanioPagina, CancellationToken cancellationToken = default);

    // Escritura

    /// <summary>
    /// Agrega una nueva facturación al contexto (requiere SaveChangesAsync posterior).
    /// </summary>
    Task AgregarAsync(FacturacionEntity facturacion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca la facturación como modificada en el contexto.
    /// </summary>
    void Actualizar(FacturacionEntity facturacion);

    /// <summary>
    /// Aplica el borrado lógico (soft delete) a la facturación especificada.
    /// </summary>
    void EliminarLogico(FacturacionEntity facturacion);
}
