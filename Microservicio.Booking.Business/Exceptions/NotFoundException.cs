namespace Microservicio.Booking.Business.Exceptions;

/// <summary>
/// Excepción lanzada cuando un recurso solicitado no existe en el sistema
/// o fue eliminado lógicamente.
/// La API la traduce a HTTP 404 Not Found.
/// </summary>
public class NotFoundException : BusinessException
{
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor de conveniencia que construye el mensaje estándar.
    /// </summary>
    public NotFoundException(string recurso, object identificador)
        : base($"{recurso} con identificador '{identificador}' no fue encontrado.")
    {
    }
}