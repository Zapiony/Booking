namespace Microservicio.Booking.DataAccess.Entities;

/// Entidad que mapea la tabla booking.cliente en PostgreSQL.
/// Relación 1:1 con usuario_app (autenticación).
/// Concurrencia optimista mediante xmin (columna del sistema de PostgreSQL).
public class ClienteEntity
{
    // Identificación técnica

    //Clave primaria interna (SERIAL). No se expone en la API.
    public int IdCliente { get; set; }

    //Identificador público UUID expuesto en la API REST
    public Guid GuidCliente { get; set; }

    // Datos funcionales

    public int IdUsuario { get; set; }

    //Nombre(s) del cliente. Solo para personas naturales.
    public string? Nombres { get; set; }

    //Apellido(s) del cliente. Solo para personas naturales.
    public string? Apellidos { get; set; }

    /// Razón social. Solo para personas jurídicas (tipo_identificacion = 'RUC').
    /// Al menos uno de Nombres o RazonSocial debe estar presente (validación en negocio).
    public string? RazonSocial { get; set; }

    //Tipo de documento de identidad.
    //Valores permitidos: CI | RUC | PASS | EXT
    public string TipoIdentificacion { get; set; } = string.Empty;

    //Número del documento de identidad. Único por tipo.
    public string NumeroIdentificacion { get; set; } = string.Empty;

    //Correo electrónico del cliente. Indexado en BD.
    public string Correo { get; set; } = string.Empty;

    //Teléfono de contacto. Opcional.
    public string? Telefono { get; set; }

    // Dirección del cliente. Opcional
    public string? Direccion { get; set; }

    //Estado y ciclo de vida

    
    // Valores: ACT = Activo | INA = Inactivo | SUS = Suspendido
    public string Estado { get; set; } = "ACT";

    //Soft delete. No se eliminan registros físicamente.
    public bool EsEliminado { get; set; } = false;

    
    //Auditoría
    

    //Usuario que creó el registro.
    public string? CreadoPorUsuario { get; set; }

    //Fecha de creación en UTC. Tipo TIMESTAMPTZ en PostgreSQL.
    public DateTimeOffset FechaRegistroUtc { get; set; }

    //Usuario que realizó la última modificación.
    public string? ModificadoPorUsuario { get; set; }

    //Fecha de la última modificación en UTC.
    public DateTimeOffset? FechaModificacionUtc { get; set; }

    //IP desde donde se realizó la última modificación.
    public string? ModificacionIp { get; set; }

    //Nombre del microservicio o canal de origen.
    public string? ServicioOrigen { get; set; }

    // Navegación

    //Propiedad de navegación hacia el usuario de autenticación.
    public UsuarioAppEntity? UsuarioApp { get; set; }
}