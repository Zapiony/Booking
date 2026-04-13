namespace Microservicio.Usuarios.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla booking.usuario_app.
/// Contiene las credenciales de autenticación del usuario en el sistema.
/// </summary>
public class UsuarioAppEntity
{
    // -------------------------------------------------------------------------
    // [1] Identificación técnica
    // -------------------------------------------------------------------------

    /// <summary>
    /// PK interna. No se expone en la API.
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Identificador público expuesto en la API REST.
    /// </summary>
    public Guid UsuarioGuid { get; set; }

    // -------------------------------------------------------------------------
    // [2] Datos funcionales
    // -------------------------------------------------------------------------

    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // [3] Seguridad
    // -------------------------------------------------------------------------

    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // [4] Estado y ciclo de vida
    // -------------------------------------------------------------------------

    /// <summary>
    /// ACT = Activo | INA = Inactivo.
    /// </summary>
    public string EstadoUsuario { get; set; } = "ACT";

    /// <summary>
    /// Borrado lógico. 0 = vigente, 1 = eliminado.
    /// </summary>
    public bool EsEliminado { get; set; } = false;

    /// <summary>
    /// Flag operativo rápido complementario al estado.
    /// </summary>
    public bool Activo { get; set; } = true;

    // -------------------------------------------------------------------------
    // [5] Auditoría
    // -------------------------------------------------------------------------

    public DateTime FechaRegistroUtc { get; set; }
    public string CreadoPorUsuario { get; set; } = string.Empty;
    public string? ModificadoPorUsuario { get; set; }
    public DateTime? FechaModificacionUtc { get; set; }

    // -------------------------------------------------------------------------
    // [6] Concurrencia optimista (ROWVERSION → EF Core token)
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
    /// Roles asignados a este usuario (vía tabla puente usuarios_roles).
    /// </summary>
    public ICollection<UsuarioRolEntity> UsuariosRoles { get; set; } = [];
}