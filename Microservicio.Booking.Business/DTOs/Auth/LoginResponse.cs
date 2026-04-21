namespace Microservicio.Booking.Business.DTOs.Auth;

/// <summary>
/// DTO de salida del proceso de autenticación.
/// Contiene la información base que la API usará para generar el JWT.
/// No incluye credenciales ni hash.
/// </summary>
public class LoginResponse
{
    public Guid UsuarioGuid { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }

    /// <summary>
    /// Roles activos del usuario. La API los incluirá como claims en el JWT.
    /// </summary>
    public IReadOnlyList<string> Roles { get; set; } = [];
}