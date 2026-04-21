namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo de datos de cliente desacoplado de EF Core.
/// Esta capa no expone entidades directamente a capas superiores.
/// </summary>
public class ClienteDataModel
{
    // [1] Identificación técnica

    public int IdCliente { get; set; }
    public Guid GuidCliente { get; set; }

    // [2] Datos funcionales

    public int IdUsuario { get; set; }

    /// <summary>Solo para personas naturales.</summary>
    public string? Nombres { get; set; }

    /// <summary>Solo para personas naturales.</summary>
    public string? Apellidos { get; set; }

    /// <summary>Solo para personas jurídicas (RUC).</summary>
    public string? RazonSocial { get; set; }

    /// <summary>CI | RUC | PASS | EXT</summary>
    public string TipoIdentificacion { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }

    // [3] Estado y ciclo de vida

    /// <summary>ACT | INA | SUS</summary>
    public string Estado { get; set; } = "ACT";

    public bool EsEliminado { get; set; }

    // [4] Auditoría

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}