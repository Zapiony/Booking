using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Queries;

/// <summary>
/// Implementación de consultas directas de solo lectura para auditoría.
/// Utiliza AsNoTracking() de EF Core.
/// </summary>
public class LogAuditoriaQueryRepository : ILogAuditoriaQueryRepository
{
    private readonly BookingDbContext _context;

    public LogAuditoriaQueryRepository(BookingDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Consultas paginadas y proyecciones
    // -------------------------------------------------------------------------

    public async Task<PagedResult<LogAuditoriaResumenDto>> ListarLogsAsync(string? tablaAfectada, int paginaActual, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        var query = _context.LogsAuditoria
            .AsNoTracking()
            .Where(l => !l.EsEliminadoLog);

        if (!string.IsNullOrWhiteSpace(tablaAfectada))
        {
            query = query.Where(l => l.TablaAfectada == tablaAfectada);
        }

        var queryProyectada = query
            .OrderByDescending(l => l.FechaUtc)
            .Select(l => new LogAuditoriaResumenDto(
                l.IdLog,
                l.TablaAfectada,
                l.Operacion,
                l.CreadoPorUsuario,
                l.FechaUtc
            ));

        var totalRegistros = await queryProyectada.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<LogAuditoriaResumenDto>.Vacio(paginaActual, tamanoPagina);

        var items = await queryProyectada
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<LogAuditoriaResumenDto>(items, totalRegistros, paginaActual, tamanoPagina);
    }
}
