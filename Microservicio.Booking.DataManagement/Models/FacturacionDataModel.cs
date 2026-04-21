namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo de datos desacoplado de EF Core que representa una facturación
/// dentro de la capa de Gestión de Datos.
/// Las capas superiores (negocio y API) trabajan con este modelo,
/// nunca con FacturacionEntity directamente.
/// </summary>
public class FacturacionDataModel
{
    // -------------------------------------------------------------------------
    // Identificación
    // -------------------------------------------------------------------------
    public int IdFactura { get; set; }
    public Guid GuidFactura { get; set; }

    // -------------------------------------------------------------------------
    // Datos funcionales
    // -------------------------------------------------------------------------
    public int IdCliente { get; set; }
    public int IdServicio { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal Total { get; set; }
    public string? ObservacionesFactura { get; set; }
    public string? OrigenCanalFactura { get; set; }

    // -------------------------------------------------------------------------
    // Estado y ciclo de vida
    // -------------------------------------------------------------------------
    public string Estado { get; set; } = "ABI";
    public bool EsEliminado { get; set; } = false;

    // -------------------------------------------------------------------------
    // Inhabilitación
    // -------------------------------------------------------------------------
    public DateTimeOffset? FechaInhabilitacionUtc { get; set; }
    public string? MotivoInhabilitacion { get; set; }
    
    // -------------------------------------------------------------------------
    // Auditoría
    // -------------------------------------------------------------------------
    public string CreadoPorUsuario { get; set; } = string.Empty;
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string ServicioOrigen { get; set; } = string.Empty;
}
