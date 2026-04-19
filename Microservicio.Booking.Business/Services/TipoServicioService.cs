using Microservicio.Booking.Business.DTOs;
using Microservicio.Booking.Business.DTOs.TipoServicio;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.Business.Services;

public sealed class TipoServicioService : ITipoServicioService
{
    private readonly ITipoServicioDataService _tipoServicioData;

    public TipoServicioService(ITipoServicioDataService tipoServicioData)
    {
        _tipoServicioData = tipoServicioData;
    }

    public async Task<TipoServicioResponse?> ObtenerPorGuidAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default)
    {
        if (guidTipoServicio == Guid.Empty)
            throw new ValidationException("GuidTipoServicio no es válido.");

        var modelo = await _tipoServicioData.ObtenerPorGuidAsync(guidTipoServicio, cancellationToken);
        return modelo is null ? null : TipoServicioBusinessMapper.ARespuesta(modelo);
    }

    public async Task<TipoServicioResponse?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ValidationException("Nombre es obligatorio.");

        var modelo = await _tipoServicioData.ObtenerPorNombreAsync(nombre.Trim(), cancellationToken);
        return modelo is null ? null : TipoServicioBusinessMapper.ARespuesta(modelo);
    }

    public async Task<IReadOnlyList<TipoServicioResponse>> ListarActivosAsync(CancellationToken cancellationToken = default)
    {
        var items = await _tipoServicioData.ListarActivosAsync(cancellationToken);
        return items.Select(TipoServicioBusinessMapper.ARespuesta).ToList();
    }

    public async Task<PagedResultado<TipoServicioResponse>> ListarPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        TipoServicioValidator.ValidarPaginacion(paginaActual, tamanoPagina);
        var pagina = await _tipoServicioData.ListarPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);
        return TipoServicioBusinessMapper.APaginado(pagina, TipoServicioBusinessMapper.ARespuesta);
    }

    public async Task<TipoServicioResponse> CrearAsync(CrearTipoServicioRequest request, CancellationToken cancellationToken = default)
    {
        TipoServicioValidator.ValidarCrear(request);
        var modelo = TipoServicioBusinessMapper.ACrearDataModel(request);

        try
        {
            var creado = await _tipoServicioData.CrearAsync(modelo, cancellationToken);
            return TipoServicioBusinessMapper.ARespuesta(creado);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }

    public async Task<TipoServicioResponse> ActualizarAsync(ActualizarTipoServicioRequest request, CancellationToken cancellationToken = default)
    {
        TipoServicioValidator.ValidarActualizar(request);

        var existente = await _tipoServicioData.ObtenerPorGuidAsync(request.GuidTipoServicio, cancellationToken);
        if (existente is null)
            throw new NotFoundException("No se encontró el tipo de servicio indicado.");

        TipoServicioBusinessMapper.AplicarActualizacion(request, existente);

        try
        {
            var actualizado = await _tipoServicioData.ActualizarAsync(existente, cancellationToken);
            return TipoServicioBusinessMapper.ARespuesta(actualizado);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }

    public async Task EliminarAsync(Guid guidTipoServicio, CancellationToken cancellationToken = default)
    {
        if (guidTipoServicio == Guid.Empty)
            throw new ValidationException("GuidTipoServicio no es válido.");

        try
        {
            await _tipoServicioData.EliminarLogicoAsync(guidTipoServicio, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }
}
