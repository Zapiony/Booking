using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Operaciones de alto nivel sobre el catálogo de tipos de servicio.
/// </summary>
public interface ITipoServicioDataService
{
    Task<TipoServicioDataModel?> ObtenerPorIdAsync(int idTipoServicio, CancellationToken cancellationToken = default);

    Task<TipoServicioDataModel?> ObtenerPorGuidAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default);

    Task<TipoServicioDataModel?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TipoServicioDataModel>> ListarActivosAsync(CancellationToken cancellationToken = default);

    Task<DataPagedResult<TipoServicioDataModel>> ListarPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    Task<TipoServicioDataModel> CrearAsync(TipoServicioDataModel modelo, CancellationToken cancellationToken = default);

    Task<TipoServicioDataModel> ActualizarAsync(TipoServicioDataModel modelo, CancellationToken cancellationToken = default);

    Task EliminarLogicoAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default);
}
