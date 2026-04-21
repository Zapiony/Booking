using Microservicio.Booking.Business.DTOs.Rol;
using Microservicio.Booking.Business.Exceptions;

namespace Microservicio.Booking.Business.Validators;

/// <summary>
/// Validador de reglas de negocio para operaciones sobre roles.
/// </summary>
public static class RolValidator
{
    public static void ValidarCrear(CrearRolRequest request)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(request.NombreRol))
            errores.Add("El nombre del rol es obligatorio.");
        else if (request.NombreRol.Length > 50)
            errores.Add("El nombre del rol no puede superar 50 caracteres.");

        if (request.DescripcionRol is not null && request.DescripcionRol.Length > 200)
            errores.Add("La descripción no puede superar 200 caracteres.");

        if (string.IsNullOrWhiteSpace(request.CreadoPorUsuario))
            errores.Add("El campo CreadoPorUsuario es obligatorio para auditoría.");

        if (errores.Count > 0)
            throw new ValidationException("La solicitud de creación de rol contiene errores.", errores);
    }

    public static void ValidarAsignacion(AsignarRolRequest request)
    {
        var errores = new List<string>();

        if (request.UsuarioGuid == Guid.Empty)
            errores.Add("El GUID del usuario es obligatorio.");

        if (request.RolGuid == Guid.Empty)
            errores.Add("El GUID del rol es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.EjecutadoPorUsuario))
            errores.Add("El campo EjecutadoPorUsuario es obligatorio para auditoría.");

        if (errores.Count > 0)
            throw new ValidationException("La solicitud de asignación de rol contiene errores.", errores);
    }
}