namespace Microservicio.Booking.Business.DTOs.Servicio;

public sealed class ServicioResponse
{
    public Guid GuidServicio { get; set; }

    public Guid GuidTipoServicio { get; set; }
    public string? TipoServicioNombre { get; set; }

    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }

    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;

    public string CorreoContacto { get; set; } = string.Empty;
    public string? TelefonoContacto { get; set; }
    public string? Direccion { get; set; }
    public string? SitioWeb { get; set; }
    public string? LogoUrl { get; set; }

    public string Estado { get; set; } = string.Empty;
    public bool EsEliminado { get; set; }

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }

    /// <summary>Para cliente que deba reenviar en actualizaciones.</summary>
    public string? RowVersionBase64 { get; set; }
}
