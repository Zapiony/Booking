// Microservicio.Booking.Business/Validators/ClienteValidator.cs

using Microservicio.Booking.Business.DTOs.Cliente;

namespace Microservicio.Booking.Business.Validators;

/// <summary>
/// Validador de reglas de negocio para clientes.
/// No usa DataAnnotations — aplica lógica funcional real.
/// Retorna una colección de errores para que el servicio
/// pueda lanzar ValidationException con todos los problemas a la vez.
/// </summary>
public static class ClienteValidator
{
    // =========================================================================
    // Valores permitidos
    // =========================================================================

    private static readonly HashSet<string> TiposIdentificacionValidos =
        new() { "CI", "RUC", "PASS", "EXT" };

    private static readonly HashSet<string> EstadosValidos =
        new() { "ACT", "INA", "SUS" };

    // =========================================================================
    // Validación de creación
    // =========================================================================

    /// <summary>
    /// Valida las reglas de negocio para crear un cliente.
    /// Retorna lista vacía si no hay errores.
    /// </summary>
    public static IReadOnlyList<string> ValidarCreacion(CrearClienteRequest request)
    {
        var errores = new List<string>();

        // Vínculo con usuario
        if (request.IdUsuario <= 0)
            errores.Add("El IdUsuario es obligatorio y debe ser mayor a cero.");

        // Persona natural vs jurídica
        ValidarNombresORazonSocial(
            request.Nombres,
            request.Apellidos,
            request.RazonSocial,
            request.TipoIdentificacion,
            errores);

        // Identificación
        ValidarIdentificacion(
            request.TipoIdentificacion,
            request.NumeroIdentificacion,
            errores);

        // Contacto
        ValidarCorreo(request.Correo, errores);
        ValidarTelefono(request.Telefono, errores);

        return errores.AsReadOnly();
    }

    // =========================================================================
    // Validación de actualización
    // =========================================================================

    /// <summary>
    /// Valida las reglas de negocio para actualizar un cliente.
    /// Retorna lista vacía si no hay errores.
    /// </summary>
    public static IReadOnlyList<string> ValidarActualizacion(ActualizarClienteRequest request)
    {
        var errores = new List<string>();

        // Identificador
        if (request.GuidCliente == Guid.Empty)
            errores.Add("El GuidCliente es obligatorio.");

        // Persona natural vs jurídica
        ValidarNombresORazonSocial(
            request.Nombres,
            request.Apellidos,
            request.RazonSocial,
            request.TipoIdentificacion,
            errores);

        // Identificación
        ValidarIdentificacion(
            request.TipoIdentificacion,
            request.NumeroIdentificacion,
            errores);

        // Contacto
        ValidarCorreo(request.Correo, errores);
        ValidarTelefono(request.Telefono, errores);

        // Estado
        if (string.IsNullOrWhiteSpace(request.Estado))
            errores.Add("El estado es obligatorio.");
        else if (!EstadosValidos.Contains(request.Estado.ToUpper()))
            errores.Add($"El estado '{request.Estado}' no es válido. Use: ACT, INA o SUS.");

        return errores.AsReadOnly();
    }

    // =========================================================================
    // Validación de cambio de estado
    // =========================================================================

    /// <summary>
    /// Valida que el nuevo estado sea un valor permitido.
    /// </summary>
    public static IReadOnlyList<string> ValidarCambioEstado(string nuevoEstado)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(nuevoEstado))
            errores.Add("El nuevo estado es obligatorio.");
        else if (!EstadosValidos.Contains(nuevoEstado.ToUpper()))
            errores.Add($"El estado '{nuevoEstado}' no es válido. Use: ACT, INA o SUS.");

        return errores.AsReadOnly();
    }

    // =========================================================================
    // Métodos privados de validación
    // =========================================================================

    private static void ValidarNombresORazonSocial(
        string? nombres,
        string? apellidos,
        string? razonSocial,
        string? tipoIdentificacion,
        List<string> errores)
    {
        var esJuridica = tipoIdentificacion?.ToUpper() == "RUC";

        if (esJuridica)
        {
            // Persona jurídica — requiere razón social
            if (string.IsNullOrWhiteSpace(razonSocial))
                errores.Add("La razón social es obligatoria para clientes con RUC.");
        }
        else
        {
            // Persona natural — requiere nombres y apellidos
            if (string.IsNullOrWhiteSpace(nombres))
                errores.Add("Los nombres son obligatorios para personas naturales.");

            if (string.IsNullOrWhiteSpace(apellidos))
                errores.Add("Los apellidos son obligatorios para personas naturales.");
        }
    }

    private static void ValidarIdentificacion(
        string? tipoIdentificacion,
        string? numeroIdentificacion,
        List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(tipoIdentificacion))
        {
            errores.Add("El tipo de identificación es obligatorio.");
        }
        else if (!TiposIdentificacionValidos.Contains(tipoIdentificacion.ToUpper()))
        {
            errores.Add($"El tipo de identificación '{tipoIdentificacion}' no es válido. " +
                        $"Use: CI, RUC, PASS o EXT.");
        }

        if (string.IsNullOrWhiteSpace(numeroIdentificacion))
            errores.Add("El número de identificación es obligatorio.");
        else if (numeroIdentificacion.Length > 20)
            errores.Add("El número de identificación no puede superar 20 caracteres.");
    }

    private static void ValidarCorreo(string? correo, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            errores.Add("El correo electrónico es obligatorio.");
            return;
        }

        if (correo.Length > 255)
            errores.Add("El correo no puede superar 255 caracteres.");

        if (!correo.Contains('@') || !correo.Contains('.'))
            errores.Add("El correo electrónico no tiene un formato válido.");
    }

    private static void ValidarTelefono(string? telefono, List<string> errores)
    {
        if (telefono is null) return;

        if (telefono.Length > 30)
            errores.Add("El teléfono no puede superar 30 caracteres.");
    }
}