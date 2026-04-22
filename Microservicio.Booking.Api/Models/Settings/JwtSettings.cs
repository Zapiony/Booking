namespace Microservicio.Booking.Api.Models.Settings;

/// <summary>
/// Configuración del JWT leída desde appsettings.json → sección "JwtSettings".
/// Se inyecta con IOptions<JwtSettings> en los controllers que generan tokens.
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationMinutes { get; set; }
}
