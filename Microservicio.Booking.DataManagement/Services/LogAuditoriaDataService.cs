using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Mappers;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Implementación del servicio de gestión de datos para auditoría.
/// Coordina repositorios a través de la UoW, mapea entidades a modelos
/// y confirma cambios.
/// </summary>
public class LogAuditoriaDataService : ILogAuditoriaDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogAuditoriaDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // =========================================================================
    // Consultas simples
    // =========================================================================

    public async Task<LogAuditoriaDataModel?> ObtenerPorIdAsync(long idLog, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.LogAuditoriaRepository.ObtenerPorIdAsync(idLog, cancellationToken);
        return entity is null ? null : LogAuditoriaDataMapper.ToDataModel(entity);
    }

    // =========================================================================
    // Consultas paginadas y filtradas
    // =========================================================================

    public async Task<DataPagedResult<LogAuditoriaDataModel>> BuscarAsync(LogAuditoriaFiltroDataModel filtro, CancellationToken cancellationToken = default)
    {
        var resultado = await _unitOfWork.LogAuditoriaQueryRepository.ListarLogsAsync(filtro.TablaAfectada, filtro.PaginaActual, filtro.TamanioPagina, cancellationToken);

        return DataPagedResult<LogAuditoriaDataModel>.DesdeDal(
            resultado,
            dto => new LogAuditoriaDataModel
            {
                IdLog = dto.IdLog,
                TablaAfectada = dto.TablaAfectada,
                Operacion = dto.Operacion,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                FechaUtc = dto.FechaUtc
            });
    }

    // =========================================================================
    // Escritura
    // =========================================================================

    public async Task<LogAuditoriaDataModel> CrearAsync(LogAuditoriaDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = LogAuditoriaDataMapper.ToEntity(model);
        await _unitOfWork.LogAuditoriaRepository.AgregarAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return LogAuditoriaDataMapper.ToDataModel(entity);
    }
}
