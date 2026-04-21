using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

public interface ILogAuditoriaDataService
{
    Task<LogAuditoriaDataModel?> ObtenerPorIdAsync(long idLog, CancellationToken cancellationToken = default);
    Task<DataPagedResult<LogAuditoriaDataModel>> BuscarAsync(LogAuditoriaFiltroDataModel filtro, CancellationToken cancellationToken = default);
    Task<LogAuditoriaDataModel> CrearAsync(LogAuditoriaDataModel model, CancellationToken cancellationToken = default);
}
