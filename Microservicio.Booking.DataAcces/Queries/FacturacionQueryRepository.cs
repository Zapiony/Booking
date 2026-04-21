using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Queries.Interfaces;
using Microservicio.Booking.DataAccess.Context;
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

    public async Task<PagedResult<FacturacionResumenDto>> ListarFacturacionesAsync(
        string? estado, 
        int? idCliente, 
        DateTime? fechaInicio, 
        DateTime? fechaFin, 
        int paginaActual, 
        int tamanoPagina, 
        CancellationToken cancellationToken = default)
    {
        var queryable = _context.Facturaciones.AsNoTracking();

        // Aplicar filtros funcionales
        if (!string.IsNullOrWhiteSpace(estado))
            queryable = queryable.Where(f => f.Estado == estado);

        if (idCliente.HasValue)
            queryable = queryable.Where(f => f.IdCliente == idCliente.Value);

        if (fechaInicio.HasValue)
            queryable = queryable.Where(f => f.FechaEmision >= fechaInicio.Value);

        if (fechaFin.HasValue)
            queryable = queryable.Where(f => f.FechaEmision <= fechaFin.Value);

        var query = queryable
            .OrderByDescending(f => f.FechaRegistroUtc)
            .Select(f => new FacturacionResumenDto(
                f.GuidFactura,
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

    public async Task<FacturacionResumenDto?> ObtenerResumenPorGuidAsync(Guid guidFactura, CancellationToken cancellationToken = default)
    {
        return await _context.Facturaciones
            .AsNoTracking()
            .Where(f => f.GuidFactura == guidFactura)
            .Select(f => new FacturacionResumenDto(
                f.GuidFactura,
                f.NumeroFactura,
                f.Total,
                f.Estado,
                f.FechaRegistroUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
