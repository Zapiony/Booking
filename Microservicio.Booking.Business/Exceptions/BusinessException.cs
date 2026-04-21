namespace Microservicio.Booking.Business.Exceptions;

/// <summary>
/// Excepción base para errores de negocio controlados.
/// Toda excepción de negocio debe heredar de esta clase.
/// La capa API la captura y la traduce a respuestas HTTP apropiadas.
/// </summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}