namespace Microservicio.Booking.Business.Exceptions;

/// <summary>
/// Entrada o regla de validación no satisfecha antes o durante la operación de negocio.
/// </summary>
public class ValidationException : BusinessException
{
    public IReadOnlyList<string> Errores { get; }

    public ValidationException(string mensaje, string? codigo = null)
        : base(mensaje, codigo)
    {
        Errores = new[] { mensaje };
    }

    public ValidationException(IReadOnlyList<string> errores, string? codigo = null)
        : base(errores.Count > 0 ? string.Join("; ", errores) : "Error de validación.", codigo)
    {
        Errores = errores.Count > 0 ? errores : Array.Empty<string>();
    }
}
