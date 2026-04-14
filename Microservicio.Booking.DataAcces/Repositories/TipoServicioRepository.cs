using Microservicio.Servicios.DataAccess.Common;
using Microservicio.Servicios.DataAccess.Entities;
using Microservicio.Servicios.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Servicios.DataAccess.Repositories;

/// <summary>
/// Implementación de ITipoServicioRepository.
/// Gestiona el catálogo cerrado de tipos de servicio (booking.tipo_servicio).
/// Nunca llama SaveChanges directamente; esa responsabilidad
/// pertenece a la unidad de trabajo de la capa superior.
/// </summary>
public class TipoServicioRepository : ITipoServicioRepository
{
    private readonly DbContext _context;

    public TipoServicioRepository(DbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable
    // -------------------------------------------------------------------------

    private IQueryable<TipoServicioEntity> QueryVigentes =>
        _context.Set<TipoServicioEntity>().Where(ts => !ts.EsEliminado);

    // -------------------------------------------------------------------------
    // Lecturas
    // -------------------------------------------------------------------------

    public async Task<TipoServicioEntity?> ObtenerPorIdAsync(
        int idTipoServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(ts => ts.IdTipoServicio == idTipoServicio, cancellationToken);
    }

    public async Task<TipoServicioEntity?> ObtenerPorGuidAsync(
        Guid guidTipoServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(ts => ts.GuidTipoServicio == guidTipoServicio, cancellationToken);
    }

    public async Task<TipoServicioEntity?> ObtenerPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(ts => ts.Nombre == nombre, cancellationToken);
    }

    public async Task<IReadOnlyList<TipoServicioEntity>> ObtenerTodosActivosAsync(
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(ts => ts.Estado == "ACT")
            .OrderBy(ts => ts.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<TipoServicioEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(ts => ts.Nombre);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<TipoServicioEntity>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<TipoServicioEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    public async Task<bool> ExisteNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<TipoServicioEntity>()
            .AnyAsync(ts => ts.Nombre == nombre, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(
        TipoServicioEntity tipoServicio,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<TipoServicioEntity>().AddAsync(tipoServicio, cancellationToken);
    }

    public void Actualizar(TipoServicioEntity tipoServicio)
    {
        _context.Set<TipoServicioEntity>().Update(tipoServicio);
    }

    public void EliminarLogico(TipoServicioEntity tipoServicio)
    {
        tipoServicio.EsEliminado = true;
        tipoServicio.Estado = "INA";

        _context.Set<TipoServicioEntity>().Update(tipoServicio);
    }
}
