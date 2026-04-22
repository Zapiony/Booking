using Microservicio.Booking.Business.DTOs.Facturacion;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Implementación del servicio de negocio para facturación.
/// Aplica validaciones, reglas de negocio y coordina
/// operaciones con la capa de gestión de datos.
/// No accede directamente a repositorios ni a EF Core.
/// </summary>
public class FacturacionService : IFacturacionService
{
    private readonly IFacturacionDataService _facturacionDataService;

    public FacturacionService(
        IFacturacionDataService facturacionDataService)
    {
        _facturacionDataService = facturacionDataService;
    }

    // =========================================================================
    // Comandos (Escritura)
    // =========================================================================

    public async Task<FacturacionResponse> CrearAsync(
        CrearFacturacionRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = FacturacionValidator.ValidarCreacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de facturación.", errors);

        var existingFactura = await _facturacionDataService.ObtenerPorNumeroAsync(request.NumeroFactura, cancellationToken);
        if (existingFactura != null)
            throw new ValidationException($"La factura con número {request.NumeroFactura} ya existe.");

        var dataModel = FacturacionBusinessMapper.ToDataModel(request);

        var resultModel = await _facturacionDataService.CrearAsync(dataModel, cancellationToken);

        return FacturacionBusinessMapper.ToResponse(resultModel);
    }

    public async Task<FacturacionResponse> ActualizarAsync(
        ActualizarFacturacionRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = FacturacionValidator.ValidarActualizacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de actualización de facturación.", errors);

        var existingModel = await _facturacionDataService.ObtenerPorGuidAsync(request.GuidFactura, cancellationToken);
        if (existingModel == null)
            throw new NotFoundException($"No se encontró la factura con guid {request.GuidFactura}");

        if (existingModel.NumeroFactura != request.NumeroFactura)
        {
            var byNumero = await _facturacionDataService.ObtenerPorNumeroAsync(request.NumeroFactura, cancellationToken);
            if (byNumero != null && byNumero.GuidFactura != request.GuidFactura)
                throw new ValidationException($"El número de factura {request.NumeroFactura} ya está en uso.");
        }

        var updatedModel = FacturacionBusinessMapper.ToDataModel(request, existingModel);

        var resultModel = await _facturacionDataService.ActualizarAsync(updatedModel, cancellationToken)
            ?? throw new BusinessException("No se pudo actualizar la facturación.");

        return FacturacionBusinessMapper.ToResponse(resultModel);
    }

    // =========================================================================
    // Consultas (Lectura)
    // =========================================================================

    public async Task<FacturacionResponse> ObtenerPorGuidAsync(
        Guid guidFactura, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con guid {guidFactura} no encontrada.");

        return FacturacionBusinessMapper.ToResponse(model);
    }

    public async Task<FacturacionResponse> ObtenerPorNumeroAsync(
        string numeroFactura, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorNumeroAsync(numeroFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con número {numeroFactura} no encontrada.");

        return FacturacionBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<FacturacionResponse>> BuscarAsync(
    FacturacionFiltroRequest request,
    CancellationToken cancellationToken = default)
    {
        var filtro = new FacturacionFiltroDataModel
        {
            Estado = request.Estado,
            IdCliente = request.IdCliente,
            FechaEmisionInicio = request.FechaEmisionInicio,
            FechaEmisionFin = request.FechaEmisionFin,
            PaginaActual = request.PageNumber,
            TamanioPagina = request.PageSize
        };

        var pagedResult = await _facturacionDataService.BuscarAsync(filtro, cancellationToken);

        // ✅ constructor con propiedades en español
        var items = pagedResult.Items
            .Select(FacturacionBusinessMapper.ToResponse)
            .ToList();

        return new DataPagedResult<FacturacionResponse>(
            items,
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    // =========================================================================
    // Operaciones de Estado
    // =========================================================================

    public async Task EliminarLogicoAsync(
        Guid guidFactura, 
        string usuarioModificador, 
        CancellationToken cancellationToken = default)
    {
        var model = await _facturacionDataService.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        if (model == null)
            throw new NotFoundException($"Facturación con guid {guidFactura} no encontrada.");

        var result = await _facturacionDataService.EliminarLogicoAsync(guidFactura, usuarioModificador, cancellationToken);
        if (!result)
            throw new BusinessException("No se pudo eliminar lógicamente la factura.");
    }
}
