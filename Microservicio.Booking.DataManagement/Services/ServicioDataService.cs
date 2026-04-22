using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Mappers;
using Microservicio.Booking.DataManagement.Models;
using Microservicio.Servicios.DataAccess.Entities;
using Microservicio.Servicios.DataAccess.Queries;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Servicio de datos para el dominio Servicio (proveedores).
/// </summary>
public sealed class ServicioDataService : IServicioDataService
{
    private readonly IUnitOfWork _uow;

    public ServicioDataService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<ServicioDataModel?> ObtenerPorIdAsync(int idServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.Servicios.ObtenerPorIdAsync(idServicio, cancellationToken);
        if (entidad is null)
            return null;

        var tipo = await _uow.TiposServicio.ObtenerPorIdAsync(entidad.IdTipoServicio, cancellationToken);
        return ServicioDataMapper.AModelo(entidad, tipo);
    }

    public async Task<ServicioDataModel?> ObtenerPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.Servicios.ObtenerPorGuidAsync(guidServicio, cancellationToken);
        if (entidad is null)
            return null;

        var tipo = await _uow.TiposServicio.ObtenerPorIdAsync(entidad.IdTipoServicio, cancellationToken);
        return ServicioDataMapper.AModelo(entidad, tipo);
    }

    public async Task<ServicioDataModel?> ObtenerConTipoPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.Servicios.ObtenerConTipoServicioPorGuidAsync(guidServicio, cancellationToken);
        return entidad is null ? null : ServicioDataMapper.AModelo(entidad);
    }

    public async Task<ServicioDataModel?> ObtenerDetallePorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        var dto = await _uow.ConsultasServicio.ObtenerDetalleAsync(guidServicio, cancellationToken);
        return dto is null ? null : ServicioDataMapper.AModeloDesdeDetalle(dto);
    }

    public async Task<DataPagedResult<ServicioResumenDataModel>> ListarOBuscarAsync(
        ServicioFiltroDataModel filtro,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filtro);
        ArgumentOutOfRangeException.ThrowIfLessThan(filtro.PaginaActual, 1, nameof(filtro.PaginaActual));
        ArgumentOutOfRangeException.ThrowIfLessThan(filtro.TamanoPagina, 1, nameof(filtro.TamanoPagina));

        var tieneTermino = !string.IsNullOrWhiteSpace(filtro.Termino);
        var tieneTipo = filtro.GuidTipoServicio.HasValue;

        if (!tieneTermino && !tieneTipo)
        {
            var r = await _uow.ConsultasServicio.ListarServiciosAsync(
                filtro.PaginaActual,
                filtro.TamanoPagina,
                cancellationToken);
            return DataPagedResult<ServicioResumenDataModel>.DesdeDal(r, ServicioDataMapper.AResumen);
        }

        if (tieneTermino && !tieneTipo)
        {
            var r = await _uow.ConsultasServicio.BuscarServiciosAsync(
                filtro.Termino!.Trim(),
                filtro.PaginaActual,
                filtro.TamanoPagina,
                cancellationToken);
            return DataPagedResult<ServicioResumenDataModel>.DesdeDal(r, ServicioDataMapper.AResumen);
        }

        var listaTipo = await _uow.ConsultasServicio.ListarServiciosPorTipoAsync(
            filtro.GuidTipoServicio!.Value,
            cancellationToken);

        if (!tieneTermino)
            return PaginarResumenesEnMemoria(listaTipo, filtro.PaginaActual, filtro.TamanoPagina);

        var term = filtro.Termino!.Trim();
        var filtrados = listaTipo
            .Where(s =>
                s.RazonSocial.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (s.NombreComercial?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        return PaginarResumenesEnMemoria(filtrados, filtro.PaginaActual, filtro.TamanoPagina);
    }

    private static DataPagedResult<ServicioResumenDataModel> PaginarResumenesEnMemoria(
        IReadOnlyList<ServicioResumenDto> items,
        int paginaActual,
        int tamanoPagina)
    {
        var total = items.Count;
        var slice = items
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(ServicioDataMapper.AResumen)
            .ToList();

        return new DataPagedResult<ServicioResumenDataModel>(slice, total, paginaActual, tamanoPagina);
    }

    public async Task<DataPagedResult<ServicioDataModel>> ListarEntidadesPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(paginaActual, 1, nameof(paginaActual));
        ArgumentOutOfRangeException.ThrowIfLessThan(tamanoPagina, 1, nameof(tamanoPagina));

        var p = await _uow.Servicios.ObtenerTodosPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);

        var modelos = new List<ServicioDataModel>(p.Items.Count);
        foreach (var entidad in p.Items)
        {
            var tipo = await _uow.TiposServicio.ObtenerPorIdAsync(entidad.IdTipoServicio, cancellationToken);
            modelos.Add(ServicioDataMapper.AModelo(entidad, tipo));
        }

        return new DataPagedResult<ServicioDataModel>(modelos, p.TotalRegistros, p.PaginaActual, p.TamanoPagina);
    }

    public async Task<ServicioDataModel> CrearAsync(ServicioDataModel modelo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modelo);

        var tipo = await ResolverTipoAsync(modelo, cancellationToken)
            ?? throw new InvalidOperationException("No se encontró el tipo de servicio indicado.");

        if (await _uow.Servicios.ExisteIdentificacionAsync(
                modelo.TipoIdentificacion,
                modelo.NumeroIdentificacion,
                cancellationToken))
        {
            throw new InvalidOperationException("Ya existe un servicio con la misma identificación.");
        }

        var entidad = ServicioDataMapper.ANuevaEntidad(modelo, tipo.IdTipoServicio);
        await _uow.Servicios.AgregarAsync(entidad, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return ServicioDataMapper.AModelo(entidad, tipo);
    }

    public async Task<ServicioDataModel> ActualizarAsync(ServicioDataModel modelo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(modelo);

        var entidad = await _uow.Servicios.ObtenerConTipoServicioPorGuidAsync(modelo.GuidServicio, cancellationToken)
            ?? throw new InvalidOperationException("No se encontró el servicio a actualizar.");

        var tipoDestino = await ResolverTipoAsync(modelo, cancellationToken);
        if (tipoDestino is not null)
            modelo.IdTipoServicio = tipoDestino.IdTipoServicio;

        ServicioDataMapper.AplicarCambios(entidad, modelo);
        _uow.Servicios.Actualizar(entidad);
        await _uow.SaveChangesAsync(cancellationToken);

        var tipo = await _uow.TiposServicio.ObtenerPorIdAsync(entidad.IdTipoServicio, cancellationToken);
        return ServicioDataMapper.AModelo(entidad, tipo);
    }

    public async Task EliminarLogicoAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        var entidad = await _uow.Servicios.ObtenerPorGuidAsync(guidServicio, cancellationToken)
            ?? throw new InvalidOperationException("No se encontró el servicio a eliminar.");

        _uow.Servicios.EliminarLogico(entidad);
        await _uow.SaveChangesAsync(cancellationToken);
    }

    private async Task<TipoServicioEntity?> ResolverTipoAsync(
        ServicioDataModel modelo,
        CancellationToken cancellationToken)
    {
        if (modelo.GuidTipoServicio != Guid.Empty)
            return await _uow.TiposServicio.ObtenerPorGuidAsync(modelo.GuidTipoServicio, cancellationToken);

        if (modelo.IdTipoServicio > 0)
            return await _uow.TiposServicio.ObtenerPorIdAsync(modelo.IdTipoServicio, cancellationToken);

        return null;
    }
}
