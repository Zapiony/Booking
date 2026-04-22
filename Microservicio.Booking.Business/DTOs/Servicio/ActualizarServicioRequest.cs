namespace Microservicio.Booking.Business.DTOs.Servicio;

public sealed class ActualizarServicioRequest
{
    public Guid GuidServicio { get; set; }

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

    public string Estado { get; set; } = "ACT";

    public string? ModificadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }

    /// <summary>Token de concurrencia en base64 (opcional si la API lo envía así).</summary>
    public string? RowVersionBase64 { get; set; }
}
