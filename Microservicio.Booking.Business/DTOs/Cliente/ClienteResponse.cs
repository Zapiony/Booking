// Microservicio.Booking.Business/DTOs/Cliente/ClienteResponse.cs

namespace Microservicio.Booking.Business.DTOs.Cliente;

/// <summary>
/// DTO de respuesta para operaciones de cliente.
/// Es lo que la API devuelve al consumidor — nunca expone entidades ni DataModels.
/// </summary>
public class ClienteResponse
{
    // =========================================================================
    // Identificación pública
    // =========================================================================

    /// <summary>Identificador público. Nunca se expone el ID interno.</summary>
    public Guid GuidCliente { get; set; }

    // =========================================================================
    // Datos del cliente
    // =========================================================================

    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? RazonSocial { get; set; }

    /// <summary>CI | RUC | PASS | EXT</summary>
    public string TipoIdentificacion { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }

    // =========================================================================
    // Estado
    // =========================================================================

    /// <summary>ACT | INA | SUS</summary>
    public string Estado { get; set; } = null!;

    // =========================================================================
    // Auditoría básica
    // =========================================================================

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
}