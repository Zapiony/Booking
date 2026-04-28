namespace Microservicio.Booking.Business.DTOs.Usuario;

public class CrearUsuarioRequest
{
    // -------------------------------------------------------------------------
    // Datos de usuario
    // -------------------------------------------------------------------------
    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NombreRol { get; set; } = string.Empty;
    public string CreadoPorUsuario { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Datos de cliente — opcionales en creación admin, obligatorios en registro
    // -------------------------------------------------------------------------

    /// <summary>CI | RUC | PASS | EXT</summary>
    public string? TipoIdentificacion { get; set; }

    public string? NumeroIdentificacion { get; set; }

    /// <summary>Obligatorio si TipoIdentificacion != RUC</summary>
    public string? Nombres { get; set; }

    /// <summary>Obligatorio si TipoIdentificacion != RUC</summary>
    public string? Apellidos { get; set; }

    /// <summary>Obligatorio si TipoIdentificacion == RUC</summary>
    public string? RazonSocial { get; set; }

    public string? Telefono { get; set; }
    public string? Direccion { get; set; }

    // -------------------------------------------------------------------------
    // Helper — indica si el request incluye datos de cliente
    // -------------------------------------------------------------------------
    public bool TieneClienteData =>
        !string.IsNullOrWhiteSpace(TipoIdentificacion) &&
        !string.IsNullOrWhiteSpace(NumeroIdentificacion);
}