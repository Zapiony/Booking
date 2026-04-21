namespace Microservicio.Booking.DataAccess.Entities;

/// <summary>
/// Entidad que mapea la tabla booking.log_auditoria en PostgreSQL.
/// Gestiona la trazabilidad y el historial de cambios en las tablas.
/// </summary>
public class LogAuditoriaEntity
{
    // Identificación técnica

    //Clave primaria interna (BIGSERIAL).
    public long IdLog { get; set; }

    // Datos del cambio

    //Nombre de la tabla que sufrió la modificación.
    public string TablaAfectada { get; set; } = string.Empty;

    //Tipo de operación: INSERT, UPDATE o DELETE.
    public string Operacion { get; set; } = string.Empty;

    //Identificador primario (PK) del registro afectado.
    public string? IdRegistro { get; set; }

    //Estado del registro antes de la modificación (Formato JSONB).
    public string? DatosAnteriores { get; set; }

    //Estado del registro después de la modificación (Formato JSONB).
    public string? DatosNuevos { get; set; }

    // Trazabilidad de origen

    //Usuario que generó el evento de auditoría.
    public string? CreadoPorUsuario { get; set; }

    //Fecha y hora en la que ocurrió la modificación (UTC).
    public DateTimeOffset FechaUtc { get; set; }

    //Dirección IP desde la que se realizó la operación.
    public string? Ip { get; set; }

    //Microservicio, aplicación o módulo desde donde se origina la acción.
    public string? ServicioOrigen { get; set; }

    //Dispositivo o equipo desde el cual se generó la petición.
    public string? EquipoOrigen { get; set; }

    // Estado y ciclo de vida

    //Borrado lógico del registro de auditoría.
    public bool EsEliminadoLog { get; set; } = false;
}
