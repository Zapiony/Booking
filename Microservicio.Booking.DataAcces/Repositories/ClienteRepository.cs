using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Repositories;

/// <summary>
/// Implementación de IClienteRepository.
/// Toda operación de escritura requiere que el llamador invoque
/// SaveChangesAsync en la unidad de trabajo (UoW) de la capa superior.
/// Este repositorio nunca llama SaveChanges directamente.
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly BookingDbContext _context;

    public ClienteRepository(BookingDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable — filtra eliminados lógicos en un solo lugar
    // -------------------------------------------------------------------------

    /// <summary>
    /// Punto de partida para todas las consultas de cliente.
    /// Excluye registros con borrado lógico aplicado (es_eliminado = true).
    /// </summary>
    private IQueryable<ClienteEntity> QueryVigentes =>
        _context.Clientes.Where(c => !c.EsEliminado);

    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    public async Task<ClienteEntity?> ObtenerPorIdAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c => c.GuidCliente == guidCliente, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario, cancellationToken);
    }
    
    public async Task<ClienteEntity?> ObtenerPorUsuarioGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c => c.UsuarioApp.UsuarioGuid == usuarioGuid, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c => c.Correo == correo, cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(c =>
                c.TipoIdentificacion == tipoIdentificacion &&
                c.NumeroIdentificacion == numeroIdentificacion,
                cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<ClienteEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanioPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(c => c.IdCliente);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<ClienteEntity>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanioPagina);
    }

    public async Task<PagedResult<ClienteEntity>> ObtenerPorEstadoPaginadoAsync(
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
            return PagedResult<ClienteEntity>.Vacio(paginaActual, tamanioPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<ClienteEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanioPagina);
    }

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    public async Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c => c.Correo == correo, cancellationToken);
    }

    public async Task<bool> ExisteNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c =>
                c.TipoIdentificacion == tipoIdentificacion &&
                c.NumeroIdentificacion == numeroIdentificacion,
                cancellationToken);
    }

    public async Task<bool> ExisteUsuarioVinculadoAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AnyAsync(c => c.IdUsuario == idUsuario, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(
        ClienteEntity cliente,
        CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public void Actualizar(ClienteEntity cliente)
    {
        // EF Core rastrea el objeto si fue obtenido en el mismo contexto.
        // Update() fuerza el tracking en caso de entidad desconectada.
        _context.Clientes.Update(cliente);
    }

    public void EliminarLogico(ClienteEntity cliente)
    {
        cliente.EsEliminado = true;
        cliente.Estado = "INA";
        cliente.ModificadoPorUsuario = cliente.ModificadoPorUsuario;

        _context.Clientes.Update(cliente);
    }

    public void CambiarEstado(ClienteEntity cliente, string nuevoEstado)
    {
        cliente.Estado = nuevoEstado;
        _context.Clientes.Update(cliente);
    }
}