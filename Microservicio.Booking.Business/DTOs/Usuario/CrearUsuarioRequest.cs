namespace Microservicio.Booking.Business.DTOs.Usuario;

/// <summary>
/// DTO de entrada para la creación de un nuevo usuario.
/// Lo recibe la API y lo pasa al servicio de negocio.
/// </summary>
public class CrearUsuarioRequest
{
    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña en texto plano. El servicio de negocio
    /// genera el hash antes de persistir.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del rol a asignar automáticamente al registrar.
    /// Ej: "ADMINISTRADOR", "VENDEDOR".
    /// </summary>
    public string NombreRol { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del usuario que ejecuta la operación (para auditoría).
    /// </summary>
    public string CreadoPorUsuario { get; set; } = string.Empty;
}