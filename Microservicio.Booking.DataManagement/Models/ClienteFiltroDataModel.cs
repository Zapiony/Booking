namespace Microservicio.Booking.DataManagement.Models;

/// <summary>
/// Modelo de filtros para búsquedas y listados paginados de clientes.
/// Lo usa ClienteDataService para consultas especializadas.
/// </summary>
public class ClienteFiltroDataModel
{
    // Filtros de búsqueda

    /// <summary>Filtrar por nombres (búsqueda parcial).</summary>
    public string? Nombres { get; set; }

    /// <summary>Filtrar por apellidos (búsqueda parcial).</summary>
    public string? Apellidos { get; set; }

    /// <summary>Filtrar por razón social (búsqueda parcial).</summary>
    public string? RazonSocial { get; set; }

    /// <summary>Filtrar por correo electrónico (búsqueda parcial).</summary>
    public string? Correo { get; set; }

    /// <summary>Filtrar por tipo de identificación: CI | RUC | PASS | EXT</summary>
    public string? TipoIdentificacion { get; set; }

    /// <summary>Filtrar por número de identificación exacto.</summary>
    public string? NumeroIdentificacion { get; set; }

    /// <summary>Filtrar por estado: ACT | INA | SUS</summary>
    public string? Estado { get; set; }

    // Paginación

    /// <summary>Número de página actual. Inicia en 1.</summary>
    public int PaginaActual { get; set; } = 1;

    /// <summary>Cantidad de registros por página.</summary>
    public int TamanioPagina { get; set; } = 10;
}