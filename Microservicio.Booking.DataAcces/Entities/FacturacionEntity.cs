namespace Microservicio.Booking.DataAccess.Entities;

/// <summary>
/// Entidad que mapea la tabla booking.facturacion en PostgreSQL.
/// Gestiona la información de los pagos y la facturación de reservas.
/// </summary>
public class FacturacionEntity
{
    // Identificación técnica

    //Clave primaria interna (SERIAL). No se expone en la API.
    public int IdFactura { get; set; }

    //Identificador público UUID expuesto en la API REST.
    public Guid GuidFactura { get; set; }

    // Datos funcionales

    //Referencia al cliente dueño de la facturación.
    public int IdCliente { get; set; }

    //Referencia a la reserva que genera esta facturación.
    public int IdReserva { get; set; }

    //Referencia al servicio facturado.
    public int IdServicio { get; set; }

    //Número único y secuencial de la facturación.
    public string NumeroFactura { get; set; } = string.Empty;

    //Fecha de emisión de la factura.
    public DateTime FechaEmision { get; set; }

    //Valor calculado antes de impuestos.
    public decimal Subtotal { get; set; }

    //Valor del IVA aplicado.
    public decimal ValorIva { get; set; }

    //Valor total de otros impuestos aplicados.
    public decimal Impuestos { get; set; }

    //Valor total final a pagar.
    public decimal Total { get; set; }

    //Observaciones de la factura.
    public string? ObservacionesFactura { get; set; }

    //Origen del canal de factura.
    public string OrigenCanalFactura { get; set; } = string.Empty;

    // Estado y ciclo de vida

    // Valores: ABI = Abierta | APR = Aprobada | INA = Inactiva
    public string Estado { get; set; } = "ABI";

    //Soft delete. No se eliminan registros físicamente.
    public bool EsEliminado { get; set; } = false;

    // Inhabilitación
    public DateTimeOffset? FechaInhabilitacionUtc { get; set; }
    public string? MotivoInhabilitacion { get; set; }
    
    // Auditoría

    //Usuario que creó el registro.
    public string? CreadoPorUsuario { get; set; }

    //Fecha de creación en UTC. Tipo TIMESTAMPTZ en PostgreSQL.
    public DateTimeOffset FechaRegistroUtc { get; set; }

    //Usuario que realizó la última modificación.
    public string? ModificadoPorUsuario { get; set; }

    //Fecha de la última modificación en UTC.
    public DateTimeOffset? FechaModificacionUtc { get; set; }

    //IP desde donde se realizó la última modificación.
    public string? ModificacionIp { get; set; }

    //Nombre del microservicio o canal de origen.
    public string? ServicioOrigen { get; set; }

    // Navegación

    //Propiedad de navegación hacia el cliente que pertenece la facturación.
    public ClienteEntity? Cliente { get; set; }
}
