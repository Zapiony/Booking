// Microservicio.Booking.Business/DTOs/Cliente/CrearClienteRequest.cs

namespace Microservicio.Booking.Business.DTOs.Cliente;

/// <summary>
/// DTO para la creación de un nuevo cliente.
/// Lo recibe la API y lo pasa a la capa de negocio.
/// </summary>
public class CrearClienteRequest
{
    // =========================================================================
    // Vínculo con usuario de autenticación
    // =========================================================================

    /// <summary>ID del usuario de autenticación vinculado (relación 1:1).</summary>
    public int IdUsuario { get; set; }

    // =========================================================================
    // Datos personales — persona natural
    // =========================================================================

    /// <summary>Obligatorio si es persona natural.</summary>
    public string? Nombres { get; set; }

    /// <summary>Obligatorio si es persona natural.</summary>
    public string? Apellidos { get; set; }

    // =========================================================================
    // Datos empresariales — persona jurídica
    // =========================================================================

    /// <summary>Obligatorio si es persona jurídica (RUC).</summary>
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
    // Auditoría — se llena desde el token JWT en el controller
    // =========================================================================

    public string? CreadoPorUsuario { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}