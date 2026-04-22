using Microservicio.Booking.Business.DTOs.Facturacion;

namespace Microservicio.Booking.Business.Validators;

public static class FacturacionValidator
{
    public static IReadOnlyCollection<string> ValidarCreacion(CrearFacturacionRequest request)
    {
        var errors = new List<string>();

        if (request.IdCliente <= 0)
            errors.Add("El Id del cliente es obligatorio y debe ser mayor a 0.");

        if (request.IdServicio <= 0)
            errors.Add("El Id del servicio es obligatorio y debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(request.NumeroFactura))
            errors.Add("El número de factura es obligatorio.");

        if (request.Subtotal < 0)
            errors.Add("El subtotal no puede ser negativo.");

        if (request.ValorIva < 0)
            errors.Add("El valor del IVA no puede ser negativo.");

        if (request.Total < 0)
            errors.Add("El total no puede ser negativo.");

        if (request.Total < request.Subtotal)
            errors.Add("El total no puede ser menor al subtotal.");

        if (string.IsNullOrWhiteSpace(request.CreadoPorUsuario))
            errors.Add("El usuario creador es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.ServicioOrigen))
            errors.Add("El servicio de origen es obligatorio.");

        return errors;
    }

    public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarFacturacionRequest request)
    {
        var errors = new List<string>();

        if (request.GuidFactura == Guid.Empty)
            errors.Add("El Guid de la factura es inválido.");

        if (string.IsNullOrWhiteSpace(request.NumeroFactura))
            errors.Add("El número de factura es obligatorio.");

        if (request.Subtotal < 0)
            errors.Add("El subtotal no puede ser negativo.");

        if (request.ValorIva < 0)
            errors.Add("El valor del IVA no puede ser negativo.");

        if (request.Total < 0)
            errors.Add("El total no puede ser negativo.");

        if (request.Total < request.Subtotal)
            errors.Add("El total no puede ser menor al subtotal.");

        if (string.IsNullOrWhiteSpace(request.ModificadoPorUsuario))
            errors.Add("El usuario modificador es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.ServicioOrigen))
            errors.Add("El servicio de origen es obligatorio.");

        return errors;
    }
}
