namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo de datos de tipo de servicio para capas superiores.
/// </summary>
public sealed class TipoServicioDataModel
{
    public int IdTipoServicio { get; set; }
    public Guid GuidTipoServicio { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public string Estado { get; set; } = "ACT";
    public bool EsEliminado { get; set; }

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }

    public byte[] RowVersion { get; set; } = [];
}
