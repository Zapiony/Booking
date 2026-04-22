namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo de datos de servicio (proveedor) para capas superiores, sin exponer la entidad EF.
/// </summary>
public sealed class ServicioDataModel
{
    public int IdServicio { get; set; }
    public Guid GuidServicio { get; set; }

    public int IdTipoServicio { get; set; }
    /// <summary>GUID público del tipo de servicio (útil en APIs).</summary>
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
    public bool EsEliminado { get; set; }

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }

    /// <summary>Token de concurrencia (no asignar manualmente en actualizaciones salvo round-trip).</summary>
    public byte[] RowVersion { get; set; } = [];

    /// <summary>Nombre del tipo de servicio (solo lectura en listados/detalle enriquecido).</summary>
    public string? TipoServicioNombre { get; set; }
}

/// <summary>
/// Vista resumida para listados (proyección tipo query/CQRS).
/// </summary>
public sealed record ServicioResumenDataModel(
    Guid GuidServicio,
    string RazonSocial,
    string? NombreComercial,
    string TipoServicioNombre,
    string Estado);
