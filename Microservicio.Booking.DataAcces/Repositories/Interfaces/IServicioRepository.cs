using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Entities;

namespace Microservicio.Booking.DataAccess.Repositories.Interfaces;

/// <summary>
/// Contrato de acceso a datos para la entidad Servicio (tabla booking.servicio).
/// Gestiona los proveedores registrados en la plataforma.
/// </summary>
public interface IServicioRepository
{
    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    /// <summary>
    /// Obtiene un servicio por su PK interna.
    /// Retorna null si no existe o está eliminado lógicamente.
    /// </summary>
    Task<ServicioEntity?> ObtenerPorIdAsync(
        int idServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un servicio por su GUID público.
    /// Retorna null si no existe o está eliminado lógicamente.
    /// </summary>
    Task<ServicioEntity?> ObtenerPorGuidAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Lecturas con navegación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Obtiene un servicio con su tipo de servicio cargado (eager loading).
    /// </summary>
    Task<ServicioEntity?> ObtenerConTipoServicioPorGuidAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna todos los servicios activos de un tipo específico.
    /// </summary>
    Task<IReadOnlyList<ServicioEntity>> ObtenerPorTipoServicioAsync(
        int idTipoServicio,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    /// <summary>
    /// Retorna una página de servicios vigentes (es_eliminado = 0).
    /// </summary>
    Task<PagedResult<ServicioEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca servicios por término parcial sobre razón social o nombre comercial.
    /// </summary>
    Task<PagedResult<ServicioEntity>> BuscarPorRazonSocialAsync(
        string termino,
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    /// <summary>
    /// Verifica si ya existe un servicio con esa combinación
    /// tipo_identificacion + numero_identificacion (sin importar estado).
    /// </summary>
    Task<bool> ExisteIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    /// <summary>
    /// Persiste un nuevo servicio (proveedor).
    /// </summary>
    Task AgregarAsync(
        ServicioEntity servicio,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca la entidad como modificada en el ChangeTracker de EF Core.
    /// </summary>
    void Actualizar(ServicioEntity servicio);

    /// <summary>
    /// Borrado lógico: setea es_eliminado = 1 y estado = 'INA'.
    /// </summary>
    void EliminarLogico(ServicioEntity servicio);
}
