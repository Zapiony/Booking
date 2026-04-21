// Microservicio.Booking.Business/DTOs/Cliente/ActualizarClienteRequest.cs

namespace Microservicio.Booking.Business.DTOs.Cliente;

/// <summary>
/// DTO para la actualización de un cliente existente.
/// El GUID identifica el cliente a modificar.
/// </summary>
public class ActualizarClienteRequest
{
    // =========================================================================
    // Identificador del cliente a actualizar
    // =========================================================================

    /// <summary>GUID público del cliente. Lo provee la API desde la ruta.</summary>
    public Guid GuidCliente { get; set; }

    // =========================================================================
    // Datos personales — persona natural
    // =========================================================================

    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }

    // =========================================================================
    // Datos empresariales — persona jurídica
    // =========================================================================

    public string? RazonSocial { get; set; }

    // =========================================================================
    // Identificación
    // =========================================================================

    /// <summary>CI | RUC | PASS | EXT</summary>
    public string TipoIdentificacion { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;

    // =========================================================================
    // Contacto
    // =========================================================================

    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }

    // =========================================================================
    // Estado
    // =========================================================================

    /// <summary>ACT | INA | SUS</summary>
    public string Estado { get; set; } = null!;

    // =========================================================================
    // Auditoría — se llena desde el token JWT en el controller
    // =========================================================================

    public string? ModificadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}