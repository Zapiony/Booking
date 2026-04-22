namespace Microservicio.Booking.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla booking.tipo_servicio.
/// Catálogo cerrado de categorías de proveedor:
/// Vuelos | Alojamiento | Atracciones | Alquiler de Carros.
/// Nuevos valores requieren migración controlada.
/// </summary>
public class TipoServicioEntity
{
    // -------------------------------------------------------------------------
    // [1] Identificación técnica
    // -------------------------------------------------------------------------

    /// <summary>
    /// PK interna. No se expone en la API.
    /// </summary>
    public int IdTipoServicio { get; set; }

    /// <summary>
    /// Identificador público expuesto en la API REST.
    /// </summary>
    public Guid GuidTipoServicio { get; set; }

    // -------------------------------------------------------------------------
    // [2] Datos funcionales
    // -------------------------------------------------------------------------

    /// <summary>
    /// Nombre del tipo de servicio.
    /// Valores permitidos: Vuelos | Alojamiento | Atracciones | Alquiler de Carros.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    // -------------------------------------------------------------------------
    // [3] Estado y ciclo de vida
    // -------------------------------------------------------------------------

    /// <summary>
    /// ACT = Activo | INA = Inactivo.
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
    // Navegación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Servicios (proveedores) clasificados bajo este tipo.
    /// </summary>
    public ICollection<ServicioEntity> Servicios { get; set; } = [];
}
