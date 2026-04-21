namespace Microservicio.Booking.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla booking.servicio.
/// Proveedores registrados en la plataforma, cada uno vinculado
/// a un tipo de servicio único (Vuelos, Alojamiento, Atracciones, Alquiler de Carros).
/// </summary>
public class ServicioEntity
{
    // -------------------------------------------------------------------------
    // [1] Identificación técnica
    // -------------------------------------------------------------------------

    /// <summary>
    /// PK interna. No se expone en la API.
    /// </summary>
    public int IdServicio { get; set; }

    /// <summary>
    /// Identificador público expuesto en la API REST.
    /// </summary>
    public Guid GuidServicio { get; set; }

    // -------------------------------------------------------------------------
    // [2] Datos funcionales
    // -------------------------------------------------------------------------

    /// <summary>
    /// FK al tipo de servicio que clasifica este proveedor.
    /// </summary>
    public int IdTipoServicio { get; set; }

    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }

    /// <summary>
    /// RUC | CI | PASS | EXT.
    /// </summary>
    public string TipoIdentificacion { get; set; } = string.Empty;

    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string CorreoContacto { get; set; } = string.Empty;
    public string? TelefonoContacto { get; set; }
    public string? Direccion { get; set; }
    public string? SitioWeb { get; set; }
    public string? LogoUrl { get; set; }

    // -------------------------------------------------------------------------
    // [3] Estado y ciclo de vida
    // -------------------------------------------------------------------------

    /// <summary>
    /// ACT = Activo | INA = Inactivo | SUS = Suspendido.
    /// </summary>
    public string Estado { get; set; } = "ACT";

    /// <summary>
    /// Borrado lógico. 0 = vigente, 1 = eliminado.
    /// </summary>
    public bool EsEliminado { get; set; } = false;

    // -------------------------------------------------------------------------
    // [4] Auditoría
    // -------------------------------------------------------------------------

    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }

    // -------------------------------------------------------------------------
    // [5] Concurrencia optimista (ROWVERSION → EF Core token)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Token de concurrencia optimista mapeado a ROWVERSION en SQL Server.
    /// EF Core lo gestiona automáticamente; nunca se asigna manualmente.
    /// </summary>
    public byte[] RowVersion { get; set; } = [];

    // -------------------------------------------------------------------------
    // Navegación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Tipo de servicio al que pertenece este proveedor.
    /// </summary>
    public TipoServicioEntity TipoServicio { get; set; } = null!;
}
