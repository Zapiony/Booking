namespace Microservicio.Booking.Business.DTOs.Auth;

/// <summary>
/// DTO de entrada para el proceso de autenticación.
/// Recibe username y contraseña en texto plano.
/// El servicio de negocio verifica el hash; nunca persiste la contraseña tal cual.
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}