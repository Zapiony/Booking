namespace Microservicio.Booking.Business.DTOs.Facturacion;

public class ActualizarFacturacionRequest
{
    public Guid GuidFactura { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal Total { get; set; }
    public string? ObservacionesFactura { get; set; }
    public string? OrigenCanalFactura { get; set; }
    
    // Auditoría
    public string ModificadoPorUsuario { get; set; } = string.Empty;
    public string ServicioOrigen { get; set; } = string.Empty;
    public string? ModificacionIp { get; set; }
}
