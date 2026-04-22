using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Mappers;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Servicio de datos para el catálogo de tipos de servicio.
/// </summary>
public sealed class TipoServicioDataService : ITipoServicioDataService
{
    private readonly IUnitOfWork _uow;

    public TipoServicioDataService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<TipoServicioDataModel?> ObtenerPorIdAsync(int idTipoServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.TipoServicioRepository.ObtenerPorIdAsync(idTipoServicio, cancellationToken);
        return entidad is null ? null : TipoServicioDataMapper.AModelo(entidad);
    }

    public async Task<TipoServicioDataModel?> ObtenerPorGuidAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.TipoServicioRepository.ObtenerPorGuidAsync(guidTipoServicio, cancellationToken);
        return entidad is null ? null : TipoServicioDataMapper.AModelo(entidad);
    }

    public async Task<TipoServicioDataModel?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.TipoServicioRepository.ObtenerPorNombreAsync(nombre, cancellationToken);
        return entidad is null ? null : TipoServicioDataMapper.AModelo(entidad);
    }

    public async Task<IReadOnlyList<TipoServicioDataModel>> ListarActivosAsync(CancellationToken cancellationToken = default)
    {
        var items = await _uow.TipoServicioRepository.ObtenerTodosActivosAsync(cancellationToken);
        return items.Select(TipoServicioDataMapper.AModelo).ToList();
    }

    public async Task<DataPagedResult<TipoServicioDataModel>> ListarPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(paginaActual, 1, nameof(paginaActual));
        ArgumentOutOfRangeException.ThrowIfLessThan(tamanoPagina, 1, nameof(tamanoPagina));

        var p = await _uow.TipoServicioRepository.ObtenerTodosPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);
        return DataPagedResult<TipoServicioDataModel>.DesdeDal(p, TipoServicioDataMapper.AModelo);
    }

    public async Task<TipoServicioDataModel> CrearAsync(TipoServicioDataModel modelo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modelo);

        if (await _uow.TipoServicioRepository.ExisteNombreAsync(modelo.Nombre, cancellationToken))
            throw new InvalidOperationException("Ya existe un tipo de servicio con el mismo nombre.");

        var entidad = TipoServicioDataMapper.ANuevaEntidad(modelo);
        await _uow.TipoServicioRepository.AgregarAsync(entidad, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return TipoServicioDataMapper.AModelo(entidad);
    }

    public async Task<TipoServicioDataModel> ActualizarAsync(TipoServicioDataModel modelo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modelo);

        var entidad = await _uow.TipoServicioRepository.ObtenerPorGuidAsync(modelo.GuidTipoServicio, cancellationToken)
            ?? throw new InvalidOperationException("No se encontró el tipo de servicio a actualizar.");

        TipoServicioDataMapper.AplicarCambios(entidad, modelo);
        _uow.TipoServicioRepository.Actualizar(entidad);
        await _uow.SaveChangesAsync(cancellationToken);

        return TipoServicioDataMapper.AModelo(entidad);
    }

    public async Task EliminarLogicoAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.TipoServicioRepository.ObtenerPorGuidAsync(guidTipoServicio, cancellationToken)
            ?? throw new InvalidOperationException("No se encontró el tipo de servicio a eliminar.");

        _uow.TipoServicioRepository.EliminarLogico(entidad);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
