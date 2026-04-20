using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.DataAccess.Repositories;

/// <summary>
/// Implementación de IUsuarioRepository.
/// Toda operación de escritura requiere que el llamador invoque
/// SaveChangesAsync en la unidad de trabajo (UoW) de la capa superior.
/// Este repositorio nunca llama SaveChanges directamente.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly UsuarioDbContext _context;

    public UsuarioRepository(UsuarioDbContext context)
    {
        _context = context;
    }

    // -------------------------------------------------------------------------
    // Query base reutilizable — filtra eliminados lógicos en un solo lugar
    // -------------------------------------------------------------------------

    /// <summary>
    /// Punto de partida para todas las consultas de usuario.
    /// Excluye registros con borrado lógico aplicado (es_eliminado = true).
    /// </summary>
    private IQueryable<UsuarioAppEntity> QueryVigentes =>
        _context.Usuarios.Where(u => !u.EsEliminado);

    // -------------------------------------------------------------------------
    // Lecturas simples
    // -------------------------------------------------------------------------

    public async Task<UsuarioAppEntity?> ObtenerPorIdAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario, cancellationToken);
    }

    public async Task<UsuarioAppEntity?> ObtenerPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(u => u.UsuarioGuid == usuarioGuid, cancellationToken);
    }

    public async Task<UsuarioAppEntity?> ObtenerPorUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(
                u => u.Username == username,
                cancellationToken);
    }

    public async Task<UsuarioAppEntity?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .FirstOrDefaultAsync(
                u => u.Correo == correo,
                cancellationToken);
    }

    public async Task<UsuarioAppEntity?> ObtenerParaActualizarAsync(
    int idUsuario,
    CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario
                                   && !u.EsEliminado, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas con roles
    // -------------------------------------------------------------------------

    public async Task<UsuarioAppEntity?> ObtenerConRolesPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        return await QueryVigentes
            .Include(u => u.UsuariosRoles.Where(ur => !ur.EsEliminado && ur.Activo))
                .ThenInclude(ur => ur.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioGuid == usuarioGuid, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Lecturas paginadas
    // -------------------------------------------------------------------------

    public async Task<PagedResult<UsuarioAppEntity>> ObtenerTodosPaginadoAsync(
        int paginaActual,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        var query = QueryVigentes.OrderBy(u => u.IdUsuario);

        var totalRegistros = await query.CountAsync(cancellationToken);

        if (totalRegistros == 0)
            return PagedResult<UsuarioAppEntity>.Vacio(paginaActual, tamanoPagina);

        var items = await query
            .Skip((paginaActual - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);

        return new PagedResult<UsuarioAppEntity>(
            items,
            totalRegistros,
            paginaActual,
            tamanoPagina);
    }

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    public async Task<bool> ExisteUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Correo == correo, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    public async Task AgregarAsync(
        UsuarioAppEntity usuario,
        CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
    }

    public void Actualizar(UsuarioAppEntity usuario)
    {
        // EF Core rastrea el objeto si fue obtenido en el mismo contexto.
        // Update() fuerza el tracking en caso de que sea una entidad desconectada.
        _context.Usuarios.Update(usuario);
    }

    public void EliminarLogico(UsuarioAppEntity usuario)
    {
        usuario.EsEliminado = true;
        usuario.EstadoUsuario = "INA";
        usuario.Activo = false;

        // EF Core detecta el cambio automáticamente si la entidad está tracked.
        // No es necesario llamar Update() explícitamente en este caso.
        _context.Usuarios.Update(usuario);
    }
}