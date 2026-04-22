using Microservicio.Booking.Business.DTOs.LogAuditoria;

namespace Microservicio.Booking.Business.Validators;

public static class LogAuditoriaValidator
{
    public static IReadOnlyCollection<string> ValidarCreacion(CrearLogAuditoriaRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.TablaAfectada))
            errors.Add("El nombre de la tabla afectada es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Operacion))
            errors.Add("El tipo de operación es obligatorio.");
        else
        {
            var operacionUpper = request.Operacion.ToUpperInvariant();
            if (operacionUpper != "INSERT" && operacionUpper != "UPDATE" && operacionUpper != "DELETE")
            {
                errors.Add("La operación debe ser INSERT, UPDATE o DELETE.");
            }
        }

        return errors;
    }
}
