using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Entities;

namespace Microservicio.Booking.DataAccess.Repositories.Interfaces;

/// <summary>
/// Contrato de acceso a datos para la entidad TipoServicio (tabla booking.tipo_servicio).
/// Gestiona el catálogo cerrado de categorías de proveedor.
/// </summary>
public interface ITipoServicioRepository
{
    // -------------------------------------------------------------------------
    // Lecturas
    // -------------------------------------------------------------------------

    /// <summary>
    /// Obtiene un tipo de servicio por su PK interna.
    /// Retorna null si no existe o está eliminado lógicamente.
    /// </summary>
    Task<TipoServicioEntity?> ObtenerPorIdAsync(
        int idTipoServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un tipo de servicio por su GUID público.
    /// Retorna null si no existe o está eliminado lógicamente.
    /// </summary>
    Task<TipoServicioEntity?> ObtenerPorGuidAsync(
        Guid guidTipoServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un tipo de servicio por su nombre exacto
    /// (ej. "Vuelos", "Alojamiento").
    /// </summary>
    Task<TipoServicioEntity?> ObtenerPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna todos los tipos de servicio activos y vigentes.
    /// </summary>
    Task<IReadOnlyList<TipoServicioEntity>> ObtenerTodosActivosAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna una página de tipos de servicio vigentes (es_eliminado = 0).
    /// </summary>
    Task<PagedResult<TipoServicioEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    /// <summary>
    /// Verifica si ya existe un tipo de servicio con ese nombre (sin importar estado).
    /// </summary>
    Task<bool> ExisteNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    /// <summary>
    /// Persiste un nuevo tipo de servicio.
    /// </summary>
    Task AgregarAsync(
        TipoServicioEntity tipoServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca la entidad como modificada en el ChangeTracker de EF Core.
    /// </summary>
    void Actualizar(TipoServicioEntity tipoServicio);

    /// <summary>
    /// Borrado lógico: setea es_eliminado = 1 y estado = 'INA'.
    /// </summary>
    void EliminarLogico(TipoServicioEntity tipoServicio);
}
