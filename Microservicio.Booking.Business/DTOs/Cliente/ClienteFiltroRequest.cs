// Microservicio.Booking.Business/DTOs/Cliente/ClienteFiltroRequest.cs

namespace Microservicio.Booking.Business.DTOs.Cliente;

/// <summary>
/// DTO de filtros para búsquedas paginadas de clientes.
/// Lo recibe la API y se traduce a ClienteFiltroDataModel en el mapper.
/// </summary>
public class ClienteFiltroRequest
{
    // =========================================================================
    // Filtros
    // =========================================================================

    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? RazonSocial { get; set; }
    public string? Correo { get; set; }

    /// <summary>CI | RUC | PASS | EXT</summary>
    public string? TipoIdentificacion { get; set; }

    public string? NumeroIdentificacion { get; set; }

    /// <summary>ACT | INA | SUS</summary>
    public string? Estado { get; set; }

    // =========================================================================
    // Paginación
    // =========================================================================

    public int PaginaActual { get; set; } = 1;
    public int TamanioPagina { get; set; } = 10;
}