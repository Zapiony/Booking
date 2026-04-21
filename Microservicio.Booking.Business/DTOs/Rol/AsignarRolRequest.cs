namespace Microservicio.Booking.Business.DTOs.Rol;

/// <summary>
/// DTO de entrada para asignar o revocar un rol a un usuario existente.
/// </summary>
public class AsignarRolRequest
{
    /// <summary>
    /// GUID público del usuario al que se asigna o revoca el rol.
    /// </summary>
    public Guid UsuarioGuid { get; set; }

    /// <summary>
    /// GUID público del rol a asignar o revocar.
    /// </summary>
    public Guid RolGuid { get; set; }

    /// <summary>
    /// Identificador del usuario que ejecuta la operación (para auditoría).
    /// </summary>
    public string EjecutadoPorUsuario { get; set; } = string.Empty;
}