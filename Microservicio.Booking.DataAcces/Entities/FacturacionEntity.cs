namespace Microservicio.Booking.DataAccess.Entities;

/// Entidad que mapea la tabla booking.facturacion en PostgreSQL.
/// Gestiona la información de los pagos y la facturación de reservas.
public class FacturacionEntity
{
    // Identificación técnica

    //Clave primaria interna (SERIAL). No se expone en la API.
    public int IdFacturacion { get; set; }

    //Identificador público UUID expuesto en la API REST.
    public Guid GuidFacturacion { get; set; }

    // Datos funcionales

    //Referencia al cliente dueño de la facturación.
    public int IdCliente { get; set; }

    //Referencia a la reserva que genera esta facturación.
    public int IdReserva { get; set; }

    //Número único y secuencial de la facturación.
    public string NumeroFactura { get; set; } = string.Empty;

    //Valor calculado antes de impuestos.
    public decimal Subtotal { get; set; }

    //Valor total de impuestos aplicados.
    public decimal Impuestos { get; set; }

    //Valor total final a pagar.
    public decimal Total { get; set; }

    // Estado y ciclo de vida

    // Valores: ACT = Activo | INA = Inactivo | CAN = Cancelado
    public string Estado { get; set; } = "ACT";

    //Soft delete. No se eliminan registros físicamente.
    public bool EsEliminado { get; set; } = false;
    
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
