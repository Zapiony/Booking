namespace Microservicio.Booking.Business.DTOs.TipoServicio;

public sealed class ActualizarTipoServicioRequest
{
    public Guid GuidTipoServicio { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "ACT";

    public string? ModificadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }

    public string? RowVersionBase64 { get; set; }
}
