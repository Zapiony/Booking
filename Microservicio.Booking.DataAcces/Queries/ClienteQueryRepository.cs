using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.;
using Microservicio.Booking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Queries;

// DTOs de proyección — viven aquí porque son contratos de solo lectura
// propios de la DAL. No son entidades ni modelos de negocio.

/// <summary>
/// Proyección plana de un cliente para listados y búsquedas.
/// No expone campos de auditoría sensibles.
/// </summary>
public sealed record ClienteResumenDto(
    Guid GuidCliente,
    string? Nombres,
    string? Apellidos,
    string? RazonSocial,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Correo,
    string? Telefono,
    string Estado
);

/// Proyección detallada de un cliente para vistas de administración.
/// Incluye campos de auditoría completos.
public sealed record ClienteDetalleDto(
    Guid GuidCliente,
    int IdUsuario,
    string? Nombres,
    string? Apellidos,
    string? RazonSocial,
    string TipoIdentificacion,
    string NumeroIdentificacion,
    string Correo,
    string? Telefono,
    string? Direccion,
    string Estado,
    bool EsEliminado,
    string? CreadoPorUsuario,
    DateTimeOffset FechaRegistroUtc,
    string? ModificadoPorUsuario,
    DateTimeOffset? FechaModificacionUtc
);

// Repositorio de consultas

/// Repositorio de solo lectura para el dominio de clientes.
/// Aplica el lado Query del patrón CQRS liviano:
///   - Nunca modifica estado.
///   - Usa proyecciones (Select) para no cargar entidades completas.
///   - AsNoTracking() en todas las consultas para máximo rendimiento.
public class ClienteQueryRepository
{
    private readonly BookingDbContext _context;

    public ClienteQueryRepository(BookingDbContext context)
    {
        _context = context;
    }

    // Query base — reutilizable internamente

    private IQueryable<ClienteEntity> QueryVigentes =>
        _context.Clientes
                .AsNoTracking()
                .Where(c => !c.EsEliminado);

    // Búsqueda y listado

    /// Retorna una página de clientes en formato resumen.
    /// Optimizado: proyecta solo los campos necesarios.
    public async Task<PagedResult<ClienteResumenDto>> ListarClientesAsync(
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(
            items,
            totalRegistros,
            paginaActual,
            tamanioPagina);
    }

    /// Retorna el detalle completo de un cliente por su GUID público.
    /// Retorna null si el cliente no existe o está eliminado lógicamente.
    public async Task<ClienteDetalleDto?> ObtenerDetalleAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Where(c => c.GuidCliente == guidCliente)
            .Select(c => new ClienteDetalleDto(
                c.GuidCliente,
                c.IdUsuario,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Direccion,
                c.Estado,
                c.EsEliminado,
                c.CreadoPorUsuario,
                c.FechaRegistroUtc,
                c.ModificadoPorUsuario,
                c.FechaModificacionUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// Búsqueda de clientes por término parcial sobre nombres, apellidos,
    /// razón social o correo. Útil para autocompletar y filtros en paneles.
    public async Task<PagedResult<ClienteResumenDto>> BuscarClientesAsync(
        string termino,
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var terminoLower = termino.Trim().ToLower();

        var query = QueryVigentes
            .Where(c =>
                (c.Nombres != null && c.Nombres.ToLower().Contains(terminoLower)) ||
                (c.Apellidos != null && c.Apellidos.ToLower().Contains(terminoLower)) ||
                (c.RazonSocial != null && c.RazonSocial.ToLower().Contains(terminoLower)) ||
                c.Correo.ToLower().Contains(terminoLower) ||
                c.NumeroIdentificacion.ToLower().Contains(terminoLower))
            .OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(
            items,
            totalRegistros,
            paginaActual,
            tamanioPagina);
    }

    /// Retorna todos los clientes filtrados por estado (ACT / INA / SUS).
    /// Útil para reportes y gestión administrativa.
    public async Task<PagedResult<ClienteResumenDto>> ListarClientesPorEstadoAsync(
        string estado,
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes
            .Where(c => c.Estado == estado)
            .OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteResumenDto>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .Select(c => new ClienteResumenDto(
                c.GuidCliente,
                c.Nombres,
                c.Apellidos,
                c.RazonSocial,
                c.TipoIdentificacion,
                c.NumeroIdentificacion,
                c.Correo,
                c.Telefono,
                c.Estado
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteResumenDto>(
            items,
            totalRegistros,
            paginaActual,
            tamanioPagina);
    }
}