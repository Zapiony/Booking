namespace Microservicio.Booking.Business.DTOs.Servicio;

public sealed class CrearServicioRequest
{
    public Guid GuidTipoServicio { get; set; }

    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }

    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;

    public string CorreoContacto { get; set; } = string.Empty;
    public string? TelefonoContacto { get; set; }
    public string? Direccion { get; set; }
    public string? SitioWeb { get; set; }
    public string? LogoUrl { get; set; }

    /// <summary>ACT | INA | SUS</summary>
    public string Estado { get; set; } = "ACT";

    public string? CreadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}
