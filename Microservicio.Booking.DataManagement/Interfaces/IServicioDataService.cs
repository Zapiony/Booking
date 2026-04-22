using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Operaciones de alto nivel sobre servicios (proveedores), usando repositorios y consultas.
/// </summary>
public interface IServicioDataService
{
    Task<ServicioDataModel?> ObtenerPorIdAsync(int idServicio, CancellationToken cancellationToken = default);

    Task<ServicioDataModel?> ObtenerPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default);

    Task<ServicioDataModel?> ObtenerConTipoPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default);

    /// <summary>Detalle enriquecido vía capa de consultas (CQRS).</summary>
    Task<ServicioDataModel?> ObtenerDetallePorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default);

    /// <summary>Listado paginado de resúmenes según filtros (usa consultas SQL cuando aplica).</summary>
    Task<DataPagedResult<ServicioResumenDataModel>> ListarOBuscarAsync(
        ServicioFiltroDataModel filtro,
        CancellationToken cancellationToken = default);

    /// <summary>Paginación sobre entidades completas (repositorio).</summary>
    Task<DataPagedResult<ServicioDataModel>> ListarEntidadesPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    Task<ServicioDataModel> CrearAsync(ServicioDataModel modelo, CancellationToken cancellationToken = default);

    Task<ServicioDataModel> ActualizarAsync(ServicioDataModel modelo, CancellationToken cancellationToken = default);

    Task EliminarLogicoAsync(Guid guidServicio, CancellationToken cancellationToken = default);
}
