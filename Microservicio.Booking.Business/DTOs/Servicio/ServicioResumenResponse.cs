namespace Microservicio.Booking.Business.DTOs.Servicio;

public sealed class ServicioResumenResponse
{
    public Guid GuidServicio { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string TipoServicioNombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
