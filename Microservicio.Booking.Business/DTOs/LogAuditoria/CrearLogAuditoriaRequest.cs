namespace Microservicio.Booking.Business.DTOs.LogAuditoria;

public class CrearLogAuditoriaRequest
{
    public string TablaAfectada { get; set; } = string.Empty;
    public string Operacion { get; set; } = string.Empty;
    public string? IdRegistro { get; set; }
    public string? DatosAnteriores { get; set; }
    public string? DatosNuevos { get; set; }
    
    // Auditoría
    public string? CreadoPorUsuario { get; set; }
    public string? Ip { get; set; }
    public string? ServicioOrigen { get; set; }
    public string? EquipoOrigen { get; set; }
}
