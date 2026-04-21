namespace Microservicio.Booking.Api.Models.Common;

/// <summary>
/// Envoltorio estándar para respuestas exitosas de la API.
/// Todas las respuestas 2xx deben usar este modelo.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Operación exitosa.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}
