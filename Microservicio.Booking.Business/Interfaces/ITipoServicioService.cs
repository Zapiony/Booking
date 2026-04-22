using Microservicio.Booking.Business.DTOs;
using Microservicio.Booking.Business.DTOs.TipoServicio;

namespace Microservicio.Booking.Business.Interfaces;

public interface ITipoServicioService
{
    Task<TipoServicioResponse?> ObtenerPorGuidAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default);

    Task<TipoServicioResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TipoServicioResponse>> ListarActivosAsync(CancellationToken cancellationToken = default);

    Task<PagedResultado<TipoServicioResponse>> ListarPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default);

    Task<TipoServicioResponse> CrearAsync(CrearTipoServicioRequest request, CancellationToken cancellationToken = default);

    Task<TipoServicioResponse> ActualizarAsync(ActualizarTipoServicioRequest request, CancellationToken cancellationToken = default);

    Task EliminarAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default);
}
