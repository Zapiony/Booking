using Microservicio.Booking.Business.DTOs.Rol;

namespace Microservicio.Booking.Business.Interfaces;

/// <summary>
/// Contrato del servicio de negocio para la gestión de roles
/// y asignaciones usuario-rol.
/// </summary>
public interface IRolService
{
    // -------------------------------------------------------------------------
    // Catálogo de roles
    // -------------------------------------------------------------------------

    Task<IReadOnlyList<RolResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<RolResponse?> ObtenerPorGuidAsync(
        Guid rolGuid,
        CancellationToken cancellationToken = default);

    Task<RolResponse> CrearAsync(
        CrearRolRequest request,
        CancellationToken cancellationToken = default);

    Task EliminarLogicoAsync(
        Guid rolGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Asignaciones usuario-rol
    // -------------------------------------------------------------------------

    Task<IReadOnlyList<RolResponse>> ObtenerRolesDeUsuarioAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default);

    Task AsignarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default);

    Task RevocarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default);
}