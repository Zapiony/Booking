namespace Microservicio.Booking.Api.Models.Common;

/// <summary>
/// Modelo estándar para respuestas de error de la API.
/// El middleware de excepciones lo usa para traducir errores de negocio a HTTP.
/// </summary>
public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();

    public static ApiErrorResponse Fail(string message, IReadOnlyCollection<string>? errors = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? Array.Empty<string>()
        };
    }
}
