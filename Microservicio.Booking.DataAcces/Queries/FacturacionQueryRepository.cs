using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Queries;

/// <summary>
/// Implementación de las consultas especializadas de facturación.
/// Utiliza AsNoTracking() de EF Core para maximizar el rendimiento.
/// </summary>
public class FacturacionQueryRepository : IFacturacionQueryRepository
{
    private readonly BookingDbContext _context;

    public FacturacionQueryRepository(BookingDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Consultas paginadas y proyecciones
    // -------------------------------------------------------------------------

    public async Task<PagedResult<FacturacionResumenDto>> ListarFacturacionesAsync(int paginaActual, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        var query = _context.Facturaciones
            .AsNoTracking() // Optimización crucial para lecturas
            .Where(f => !f.EsEliminado)
            .OrderByDescending(f => f.FechaRegistroUtc)
            .Select(f => new FacturacionResumenDto(
                f.GuidFacturacion,
                f.NumeroFactura,
                f.Total,
                f.Estado,
                f.FechaRegistroUtc
            ));

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<FacturacionResumenDto>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);
            
        return new PagedResult<FacturacionResumenDto>(items, totalRegistros, paginaActual, tamanoPagina);
    }

    // -------------------------------------------------------------------------
    // Consultas individuales
    // -------------------------------------------------------------------------

    public async Task<FacturacionResumenDto?> ObtenerResumenPorGuidAsync(Guid guidFacturacion, CancellationToken cancellationToken = default)
    {
        return await _context.Facturaciones
            .AsNoTracking()
            .Where(f => !f.EsEliminado && f.GuidFacturacion == guidFacturacion)
            .Select(f => new FacturacionResumenDto(
                f.GuidFacturacion,
                f.NumeroFactura,
                f.Total,
                f.Estado,
                f.FechaRegistroUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
