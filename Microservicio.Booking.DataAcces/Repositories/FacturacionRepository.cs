using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Repositories;

/// <summary>
/// Implementación de IFacturacionRepository.
/// Toda operación de escritura requiere que el llamador invoque
/// SaveChangesAsync en la unidad de trabajo (UoW) de la capa superior.
/// Este repositorio nunca llama SaveChanges directamente.
/// </summary>
public class FacturacionRepository : IFacturacionRepository
{
    private readonly BookingDbContext _context;

    public FacturacionRepository(BookingDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable — filtra eliminados lógicos en un solo lugar
    // -------------------------------------------------------------------------

    /// <summary>
    /// Punto de partida para todas las consultas de facturación.
    /// Excluye registros con borrado lógico aplicado (es_eliminado = true).
    /// </summary>
    private IQueryable<FacturacionEntity> QueryVigentes => _context.Facturaciones.Where(f => !f.EsEliminado);

    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    public async Task<FacturacionEntity?> ObtenerPorIdAsync(int idFacturacion, CancellationToken cancellationToken = default)
    {
        return await QueryVigentes.FirstOrDefaultAsync(f => f.IdFacturacion == idFacturacion, cancellationToken);
    }

    public async Task<FacturacionEntity?> ObtenerPorGuidAsync(Guid guidFacturacion, CancellationToken cancellationToken = default)
    {
        return await QueryVigentes.FirstOrDefaultAsync(f => f.GuidFacturacion == guidFacturacion, cancellationToken);
    }

    public async Task<FacturacionEntity?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken cancellationToken = default)
    {
        return await QueryVigentes.FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<FacturacionEntity>> ObtenerTodosPaginadoAsync(int paginaActual, int tamanioPagina, CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(f => f.IdFacturacion);
        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<FacturacionEntity>.Vacio(paginaActual, tamanioPagina);

        var items = await query.Skip((paginaActual - 1) * tamanioPagina).Take(tamanioPagina).ToListAsync(cancellationToken);
        return new PagedResult<FacturacionEntity>(items, totalRegistros, paginaActual, tamanioPagina);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(FacturacionEntity facturacion, CancellationToken cancellationToken = default)
    {
        await _context.Facturaciones.AddAsync(facturacion, cancellationToken);
    }

    public void Actualizar(FacturacionEntity facturacion)
    {
        // EF Core rastrea el objeto si fue obtenido en el mismo contexto.
        // Update() fuerza el tracking en caso de entidad desconectada.
        _context.Facturaciones.Update(facturacion);
    }

    public void EliminarLogico(FacturacionEntity facturacion)
    {
        facturacion.EsEliminado = true;
        facturacion.Estado = "INA";
        _context.Facturaciones.Update(facturacion);
    }
}
