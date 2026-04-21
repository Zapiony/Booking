using Microservicio.Booking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Booking.DataAccess.Configurations;

/// <summary>
/// Configuración de mapeo de Entity Framework Core para LogAuditoriaEntity.
/// Define nombres de tabla, columnas, índices y tipos especiales como JSONB.
/// </summary>
public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoriaEntity>
{
    public void Configure(EntityTypeBuilder<LogAuditoriaEntity> builder)
    {
        // Tabla y esquema
        builder.ToTable("log_auditoria", "booking");

        // [1] Clave primaria
        builder.HasKey(e => e.IdLog);

        builder.Property(e => e.IdLog)
            .HasColumnName("id_log")
            .ValueGeneratedOnAdd();

        // [2] Datos del cambio
        builder.Property(e => e.TablaAfectada)
            .HasColumnName("tabla_afectada")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Operacion)
            .HasColumnName("operacion")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.IdRegistro)
            .HasColumnName("id_registro")
            .HasColumnType("text");

        builder.Property(e => e.DatosAnteriores)
            .HasColumnName("datos_anteriores")
            .HasColumnType("jsonb");

        builder.Property(e => e.DatosNuevos)
            .HasColumnName("datos_nuevos")
            .HasColumnType("jsonb");

        // [3] Trazabilidad de origen
        builder.Property(e => e.CreadoPorUsuario)
            .HasColumnName("creado_por_usuario")
            .HasMaxLength(150);

        builder.Property(e => e.FechaUtc)
            .HasColumnName("fecha_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(e => e.Ip)
            .HasColumnName("ip")
            .HasMaxLength(45);

        builder.Property(e => e.ServicioOrigen)
            .HasColumnName("servicio_origen")
            .HasMaxLength(100);

        builder.Property(e => e.EquipoOrigen)
            .HasColumnName("equipo_origen")
            .HasMaxLength(100);

        // [4] Estado y ciclo de vida
        builder.Property(e => e.EsEliminadoLog)
            .HasColumnName("es_eliminado_log")
            .HasDefaultValue(false)
            .IsRequired();

        // Índices de consulta frecuente
        builder.HasIndex(e => e.TablaAfectada)
            .HasDatabaseName("idx_log_tabla");

        builder.HasIndex(e => e.Operacion)
            .HasDatabaseName("idx_log_operacion");

        builder.HasIndex(e => e.FechaUtc)
            .HasDatabaseName("idx_log_fecha");

        builder.HasIndex(e => e.CreadoPorUsuario)
            .HasDatabaseName("idx_log_usuario");

        // Filtro global — soft delete
        builder.HasQueryFilter(e => !e.EsEliminadoLog);
    }
}
