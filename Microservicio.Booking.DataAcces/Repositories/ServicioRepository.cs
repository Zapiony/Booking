using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Repositories;

/// <summary>
/// Implementación de IServicioRepository.
/// Gestiona los proveedores registrados (booking.servicio).
/// Nunca llama SaveChanges directamente; esa responsabilidad
/// pertenece a la unidad de trabajo de la capa superior.
/// </summary>
public class ServicioRepository : IServicioRepository
{
    private readonly DbContext _context;

    public ServicioRepository(DbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable
    // -------------------------------------------------------------------------

    private IQueryable<ServicioEntity> QueryVigentes =>
        _context.Set<ServicioEntity>().Where(s => !s.EsEliminado);

    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    public async Task<ServicioEntity?> ObtenerPorIdAsync(
        int idServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(s => s.IdServicio == idServicio, cancellationToken);
    }

    public async Task<ServicioEntity?> ObtenerPorGuidAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(s => s.GuidServicio == guidServicio, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas con navegación
    // -------------------------------------------------------------------------

    public async Task<ServicioEntity?> ObtenerConTipoServicioPorGuidAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Include(s => s.TipoServicio)
            .FirstOrDefaultAsync(s => s.GuidServicio == guidServicio, cancellationToken);
    }

    public async Task<IReadOnlyList<ServicioEntity>> ObtenerPorTipoServicioAsync(
        int idTipoServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(s => s.IdTipoServicio == idTipoServicio && s.Estado == "ACT")
            .OrderBy(s => s.RazonSocial)
            .ToListAsync(cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<ServicioEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(s => s.RazonSocial);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ServicioEntity>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<ServicioEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    public async Task<PagedResult<ServicioEntity>> BuscarPorRazonSocialAsync(
        string termino,
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var terminoNormalizado = termino.Trim();
        var patron = $"%{terminoNormalizado}%";
        var terminoNormalizado = termino.Trim();
        var patron = $"%{terminoNormalizado}%";

        var query = QueryVigentes
            .Where(s =>
                EF.Functions.ILike(s.RazonSocial, patron) ||
                (s.NombreComercial != null && EF.Functions.ILike(s.NombreComercial, patron)))
                EF.Functions.ILike(s.RazonSocial, patron) ||
                (s.NombreComercial != null && EF.Functions.ILike(s.NombreComercial, patron)))
            .OrderBy(s => s.RazonSocial);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ServicioEntity>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<ServicioEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    public async Task<bool> ExisteIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<ServicioEntity>()
            .AnyAsync(
                s => s.TipoIdentificacion == tipoIdentificacion &&
                     s.NumeroIdentificacion == numeroIdentificacion,
                cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(
        ServicioEntity servicio,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<ServicioEntity>().AddAsync(servicio, cancellationToken);
    }

    public void Actualizar(ServicioEntity servicio)
    {
        _context.Set<ServicioEntity>().Update(servicio);
    }

    public void EliminarLogico(ServicioEntity servicio)
    {
        servicio.EsEliminado = true;
        servicio.Estado = "INA";

        _context.Set<ServicioEntity>().Update(servicio);
    }
}
