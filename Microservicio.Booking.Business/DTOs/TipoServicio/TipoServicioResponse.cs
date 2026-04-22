namespace Microservicio.Booking.Business.DTOs.TipoServicio;

public sealed class TipoServicioResponse
{
    public Guid GuidTipoServicio { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public string Estado { get; set; } = string.Empty;
    public bool EsEliminado { get; set; }

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }

    public string? RowVersionBase64 { get; set; }
}
