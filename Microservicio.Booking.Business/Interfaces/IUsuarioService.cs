using Microservicio.Booking.Business.DTOs.Usuario;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Interfaces;

/// <summary>
/// Contrato del servicio de negocio para la gestión de usuarios.
/// Define los casos de uso que expone la capa de negocio a la API.
/// La API depende de esta interfaz, nunca de la implementación directa.
/// </summary>
public interface IUsuarioService
{
    // -------------------------------------------------------------------------
    // Consultas
    // -------------------------------------------------------------------------

    Task<UsuarioResponse?> ObtenerPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default);

    Task<DataPagedResult<UsuarioResponse>> BuscarAsync(
        UsuarioFiltroRequest filtro,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Comandos
    // -------------------------------------------------------------------------

    /// <summary>
    /// Registra un nuevo usuario, genera el hash de contraseña
    /// y le asigna el rol indicado automáticamente.
    /// </summary>
    Task<UsuarioResponse> CrearAsync(
        CrearUsuarioRequest request,
        CancellationToken cancellationToken = default);

    Task<UsuarioResponse> ActualizarAsync(
        ActualizarUsuarioRequest request,
        CancellationToken cancellationToken = default);

    Task EliminarLogicoAsync(
        Guid usuarioGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);
}