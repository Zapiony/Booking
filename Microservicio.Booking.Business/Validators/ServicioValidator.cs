using Microservicio.Booking.Business.DTOs.Servicio;
using Microservicio.Booking.Business.Exceptions;

namespace Microservicio.Booking.Business.Validators;

/// <summary>
/// Reglas de validación de negocio para servicios (proveedores).
/// </summary>
public static class ServicioValidator
{
    public const int TamanoPaginaMaximo = 100;

    private static readonly HashSet<string> EstadosValidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "ACT", "INA", "SUS"
    };

    public static void ValidarPaginacion(int paginaActual, int tamanoPagina)
    {
        var errores = new List<string>();
        if (paginaActual < 1)
            errores.Add("PaginaActual debe ser mayor o igual a 1.");
        if (tamanoPagina < 1)
            errores.Add("TamanoPagina debe ser mayor o igual a 1.");
        if (tamanoPagina > TamanoPaginaMaximo)
            errores.Add($"TamanoPagina no puede superar {TamanoPaginaMaximo}.");

        if (errores.Count > 0)
            throw new ValidationException("Validación de paginación inválida.", errores);
    }

    public static void ValidarCrear(CrearServicioRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var errores = new List<string>();

        if (request.GuidTipoServicio == Guid.Empty)
            errores.Add("GuidTipoServicio es obligatorio.");

        ValidarCamposComunes(request.RazonSocial, request.TipoIdentificacion, request.NumeroIdentificacion,
            request.CorreoContacto, request.Estado, errores);

        if (errores.Count > 0)
            throw new ValidationException("Validación de creación de servicio inválida.", errores);
    }

    public static void ValidarActualizar(ActualizarServicioRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var errores = new List<string>();

        if (request.GuidServicio == Guid.Empty)
            errores.Add("GuidServicio es obligatorio.");
        if (request.GuidTipoServicio == Guid.Empty)
            errores.Add("GuidTipoServicio es obligatorio.");

        ValidarCamposComunes(request.RazonSocial, request.TipoIdentificacion, request.NumeroIdentificacion,
            request.CorreoContacto, request.Estado, errores);

        if (errores.Count > 0)
            throw new ValidationException("Validación de actualización de servicio inválida.", errores);
    }

    public static void ValidarFiltro(ServicioFiltroRequest filtro)
    {
        ArgumentNullException.ThrowIfNull(filtro);
        ValidarPaginacion(filtro.PaginaActual, filtro.TamanoPagina);
    }

    private static void ValidarCamposComunes(
        string razonSocial,
        string tipoIdentificacion,
        string numeroIdentificacion,
        string correo,
        string estado,
        List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(razonSocial))
            errores.Add("RazonSocial es obligatorio.");
        if (string.IsNullOrWhiteSpace(tipoIdentificacion))
            errores.Add("TipoIdentificacion es obligatorio.");
        if (string.IsNullOrWhiteSpace(numeroIdentificacion))
            errores.Add("NumeroIdentificacion es obligatorio.");
        if (string.IsNullOrWhiteSpace(correo))
            errores.Add("CorreoContacto es obligatorio.");
        else if (!CorreoPareceValido(correo))
            errores.Add("CorreoContacto no tiene un formato válido.");

        if (string.IsNullOrWhiteSpace(estado))
            errores.Add("Estado es obligatorio.");
        else if (!EstadosValidos.Contains(estado.Trim()))
            errores.Add("Estado debe ser ACT, INA o SUS.");
    }

    private static bool CorreoPareceValido(string correo)
    {
        var t = correo.Trim();
        return t.Length >= 5 && t.Contains('@', StringComparison.Ordinal) && t.Contains('.', StringComparison.Ordinal);
    }
}
