using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de datos para el dominio de usuarios.
/// Define operaciones de persistencia orientadas a la capa de negocio.
/// La capa de negocio depende de esta interfaz, nunca de EF Core directamente.
/// </summary>
public interface IUsuarioDataService
{
    // -------------------------------------------------------------------------
    // Lecturas
    // -------------------------------------------------------------------------

    Task<UsuarioDataModel?> ObtenerPorIdAsync(
        int idUsuario,
        CancellationToken cancellationToken = default);

    Task<UsuarioDataModel?> ObtenerPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default);

    Task<UsuarioDataModel?> ObtenerPorUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<UsuarioDataModel?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    Task<DataPagedResult<UsuarioDataModel>> BuscarAsync(
        UsuarioFiltroDataModel filtro,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Método exclusivo para autenticación. Devuelve hash y salt del usuario
    /// indicado. No se usa para ningún otro propósito.
    /// Retorna null si el usuario no existe o está eliminado.
    /// </summary>
    Task<(string PasswordHash, string PasswordSalt)?> ObtenerCredencialesParaAuthAsync(
        string username,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Verificaciones
    // -------------------------------------------------------------------------

    Task<bool> ExisteUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);



    // -------------------------------------------------------------------------
    // Escritura
    // -------------------------------------------------------------------------

    Task<UsuarioDataModel> CrearAsync(
        UsuarioDataModel model,
        string passwordHash,
        string passwordSalt,
        CancellationToken cancellationToken = default);

    Task<UsuarioDataModel?> ActualizarAsync(
        UsuarioDataModel model,
        CancellationToken cancellationToken = default);

    Task<bool> EliminarLogicoAsync(
        int idUsuario,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);
}