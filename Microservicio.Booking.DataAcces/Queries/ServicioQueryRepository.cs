using Microservicio.Booking.DataAccess.Common;
using Microservicio.Servicios.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Servicios.DataAccess.Queries;

/// <summary>
/// Repositorio de solo lectura para el dominio de servicios (proveedores).
/// Aplica el lado Query del patrón CQRS liviano:
///   - Nunca modifica estado.
///   - Usa proyecciones (Select) para no cargar entidades completas.
///   - AsNoTracking() en todas las consultas para máximo rendimiento.
/// </summary>
public class ServicioQueryRepository : IServicioQueryRepository
{
    private readonly DbContext _context;

    public ServicioQueryRepository(DbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base — reutilizable internamente
    // -------------------------------------------------------------------------

    private IQueryable<ServicioEntity> QueryVigentes =>
        _context.Set<ServicioEntity>()
                .AsNoTracking()
                .Where(s => !s.EsEliminado);

    // -------------------------------------------------------------------------
    // Consultas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<ServicioResumenDto>> ListarServiciosAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(s => s.RazonSocial);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ServicioResumenDto>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(s => new ServicioResumenDto(
                s.GuidServicio,
                s.RazonSocial,
                s.NombreComercial,
                s.TipoServicio.Nombre,
                s.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ServicioResumenDto>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    public async Task<ServicioDetalleDto?> ObtenerDetalleAsync(
        Guid guidServicio,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(s => s.GuidServicio == guidServicio)
            .Select(s => new ServicioDetalleDto(
                s.GuidServicio,
                s.RazonSocial,
                s.NombreComercial,
                s.TipoIdentificacion,
                s.NumeroIdentificacion,
                s.CorreoContacto,
                s.TelefonoContacto,
                s.Direccion,
                s.SitioWeb,
                s.LogoUrl,
                s.Estado,
                s.EsEliminado,
                s.TipoServicio.GuidTipoServicio,
                s.TipoServicio.Nombre,
                s.CreadoPorUsuario,
                s.FechaRegistroUtc,
                s.ModificadoPorUsuario,
                s.FechaModificacionUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<ServicioResumenDto>> BuscarServiciosAsync(
        string termino,
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var terminoNormalizado = termino.Trim();
        var patron = $"%{terminoNormalizado}%";

        var query = QueryVigentes
            .Where(s =>
                EF.Functions.ILike(s.RazonSocial, patron) ||
                (s.NombreComercial != null && EF.Functions.ILike(s.NombreComercial, patron)))
            .OrderBy(s => s.RazonSocial);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ServicioResumenDto>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(s => new ServicioResumenDto(
                s.GuidServicio,
                s.RazonSocial,
                s.NombreComercial,
                s.TipoServicio.Nombre,
                s.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ServicioResumenDto>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    public async Task<IReadOnlyList<ServicioResumenDto>> ListarServiciosPorTipoAsync(
        Guid tipoServicioGuid,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(s =>
                s.TipoServicio.GuidTipoServicio == tipoServicioGuid &&
                s.Estado == "ACT")
            .OrderBy(s => s.RazonSocial)
            .Select(s => new ServicioResumenDto(
                s.GuidServicio,
                s.RazonSocial,
                s.NombreComercial,
                s.TipoServicio.Nombre,
                s.Estado
            ))
            .ToListAsync(cancellationToken);
    }
}
