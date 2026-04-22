using Microservicio.Booking.Business.DTOs.LogAuditoria;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Implementación del servicio de negocio para logs de auditoría.
/// Aplica validaciones básicas y coordina operaciones
/// con la capa de gestión de datos para trazabilidad.
/// No accede directamente a repositorios ni a EF Core.
/// </summary>
public class LogAuditoriaService : ILogAuditoriaService
{
    private readonly ILogAuditoriaDataService _logAuditoriaDataService;

    public LogAuditoriaService(
        ILogAuditoriaDataService logAuditoriaDataService)
    {
        _logAuditoriaDataService = logAuditoriaDataService;
    }

    // =========================================================================
    // Comandos (Escritura)
    // =========================================================================

    public async Task<LogAuditoriaResponse> CrearAsync(
        CrearLogAuditoriaRequest request, 
        CancellationToken cancellationToken = default)
    {
        var errors = LogAuditoriaValidator.ValidarCreacion(request);
        if (errors.Any())
            throw new ValidationException("Error en validación de log de auditoría.", errors);

        var dataModel = LogAuditoriaBusinessMapper.ToDataModel(request);

        var resultModel = await _logAuditoriaDataService.CrearAsync(dataModel, cancellationToken);

        return LogAuditoriaBusinessMapper.ToResponse(resultModel);
    }

    // =========================================================================
    // Consultas (Lectura)
    // =========================================================================

    public async Task<DataPagedResult<LogAuditoriaResponse>> BuscarAsync(
        LogAuditoriaFiltroRequest request, 
        CancellationToken cancellationToken = default)
    {
        var filtro = new LogAuditoriaFiltroDataModel
        {
            TablaAfectada = request.TablaAfectada,
            PaginaActual = request.PageNumber,
            TamanioPagina = request.PageSize
        };

        var pagedResult = await _logAuditoriaDataService.BuscarAsync(filtro, cancellationToken);

        var items = pagedResult.Items
            .Select(LogAuditoriaBusinessMapper.ToResponse)
            .ToList();

        return new DataPagedResult<LogAuditoriaResponse>
        {
            Items = items,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalRecords = pagedResult.TotalRecords
        };
    }
}
