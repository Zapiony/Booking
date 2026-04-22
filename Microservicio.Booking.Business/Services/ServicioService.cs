using Microservicio.Booking.Business.DTOs;
using Microservicio.Booking.Business.DTOs.Servicio;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.Business.Services;

public sealed class ServicioService : IServicioService
{
    private readonly IServicioDataService _servicioData;

    public ServicioService(IServicioDataService servicioData)
    {
        _servicioData = servicioData;
    }

    public async Task<ServicioResponse?> ObtenerPorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        if (guidServicio == Guid.Empty)
            throw new ValidationException("GuidServicio no es válido.");

        var modelo = await _servicioData.ObtenerPorGuidAsync(guidServicio, cancellationToken);
        return modelo is null ? null : ServicioBusinessMapper.ARespuesta(modelo);
    }

    public async Task<ServicioResponse?> ObtenerDetallePorGuidAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        if (guidServicio == Guid.Empty)
            throw new ValidationException("GuidServicio no es válido.");

        var modelo = await _servicioData.ObtenerDetallePorGuidAsync(guidServicio, cancellationToken);
        return modelo is null ? null : ServicioBusinessMapper.ARespuesta(modelo);
    }

    public async Task<PagedResultado<ServicioResumenResponse>> ListarOBuscarAsync(
        ServicioFiltroRequest filtro,
        CancellationToken cancellationToken = default)
    {
        ServicioValidator.ValidarFiltro(filtro);
        var filtroData = ServicioBusinessMapper.AFiltroData(filtro);
        var pagina = await _servicioData.ListarOBuscarAsync(filtroData, cancellationToken);
        return ServicioBusinessMapper.APaginado(pagina, ServicioBusinessMapper.AResumenRespuesta);
    }

    public async Task<PagedResultado<ServicioResponse>> ListarEntidadesPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        ServicioValidator.ValidarPaginacion(paginaActual, tamanoPagina);
        var pagina = await _servicioData.ListarEntidadesPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);
        return ServicioBusinessMapper.APaginado(pagina, ServicioBusinessMapper.ARespuesta);
    }

    public async Task<ServicioResponse> CrearAsync(CrearServicioRequest request, CancellationToken cancellationToken = default)
    {
        ServicioValidator.ValidarCrear(request);
        var modelo = ServicioBusinessMapper.ACrearDataModel(request);

        try
        {
            var creado = await _servicioData.CrearAsync(modelo, cancellationToken);
            return ServicioBusinessMapper.ARespuesta(creado);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }

    public async Task<ServicioResponse> ActualizarAsync(ActualizarServicioRequest request, CancellationToken cancellationToken = default)
    {
        ServicioValidator.ValidarActualizar(request);

        var existente = await _servicioData.ObtenerConTipoPorGuidAsync(request.GuidServicio, cancellationToken);
        if (existente is null)
            throw new NotFoundException("No se encontró el servicio indicado.");

        ServicioBusinessMapper.AplicarActualizacion(request, existente);

        try
        {
            var actualizado = await _servicioData.ActualizarAsync(existente, cancellationToken);
            return ServicioBusinessMapper.ARespuesta(actualizado);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }

    public async Task EliminarAsync(Guid guidServicio, CancellationToken cancellationToken = default)
    {
        if (guidServicio == Guid.Empty)
            throw new ValidationException("GuidServicio no es válido.");

        try
        {
            await _servicioData.EliminarLogicoAsync(guidServicio, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            DataServiceExceptionMapper.PropagarSiInvalidOperation(ex);
            throw;
        }
    }
}
