namespace Microservicio.Booking.Business.DTOs.TipoServicio;

public sealed class CrearTipoServicioRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "ACT";
    public string? CreadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}
