namespace Microservicio.Booking.Business.DTOs.Facturacion;

/// <summary>
/// Modelo de datos (DTO) para la solicitud de creación de una nueva factura.
/// Define los datos obligatorios requeridos desde el exterior.
/// </summary>
public class CrearFacturacionRequest
{
    public int IdCliente { get; set; }
    public int IdServicio { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal Total { get; set; }
    public string? ObservacionesFactura { get; set; }
    public string? OrigenCanalFactura { get; set; }
    
    // Auditoría
    public string CreadoPorUsuario { get; set; } = string.Empty;
    public string ServicioOrigen { get; set; } = string.Empty;
    public string? ModificacionIp { get; set; }
}
