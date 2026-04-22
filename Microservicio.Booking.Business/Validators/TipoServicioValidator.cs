using Microservicio.Booking.Business.DTOs.TipoServicio;
using Microservicio.Booking.Business.Exceptions;

namespace Microservicio.Booking.Business.Validators;

public static class TipoServicioValidator
{
    public const int TamanoPaginaMaximo = 100;

    private static readonly HashSet<string> EstadosValidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "ACT", "INA"
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
            throw new ValidationException("Error de validación de paginación.", errores);
    }

    public static void ValidarCrear(CrearTipoServicioRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Nombre))
            errores.Add("Nombre es obligatorio.");

        ValidarEstado(request.Estado, errores);

        if (errores.Count > 0)
            throw new ValidationException("Error de validación al crear tipo de servicio.", errores);
    }

    public static void ValidarActualizar(ActualizarTipoServicioRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var errores = new List<string>();

        if (request.GuidTipoServicio == Guid.Empty)
            errores.Add("GuidTipoServicio es obligatorio.");
        if (string.IsNullOrWhiteSpace(request.Nombre))
            errores.Add("Nombre es obligatorio.");

        ValidarEstado(request.Estado, errores);

        if (errores.Count > 0)
            throw new ValidationException("Error de validación al actualizar tipo de servicio.", errores);
    }

    private static void ValidarEstado(string estado, List<string> errores)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            errores.Add("Estado es obligatorio.");
            return;
        }

        if (!EstadosValidos.Contains(estado.Trim()))
            errores.Add("Estado debe ser ACT o INA.");
    }
}
