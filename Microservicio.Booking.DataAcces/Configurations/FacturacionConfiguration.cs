using Microservicio.Booking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Booking.DataAccess.Configurations;

/// <summary>
/// Configuración de mapeo de Entity Framework Core para la entidad FacturacionEntity.
/// Define nombres de tabla, columnas, restricciones, índices y relaciones.
/// </summary>
public class FacturacionConfiguration : IEntityTypeConfiguration<FacturacionEntity>
{
    public void Configure(EntityTypeBuilder<FacturacionEntity> builder)
    {
        // Tabla y esquema
        builder.ToTable("facturacion", "booking");

        // [1] Clave primaria e identificación
        builder.HasKey(e => e.IdFactura);

        builder.Property(e => e.IdFactura)
            .HasColumnName("id_factura")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GuidFactura)
            .HasColumnName("guid_factura")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        // [2] Datos funcionales
        builder.Property(e => e.IdCliente)
            .HasColumnName("id_cliente")
            .IsRequired();


        builder.Property(e => e.IdServicio)
            .HasColumnName("id_servicio")
            .IsRequired();

        builder.Property(e => e.NumeroFactura)
            .HasColumnName("numero_factura")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.FechaEmision)
            .HasColumnName("fecha_emision")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(e => e.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(e => e.ValorIva)
            .HasColumnName("valor_iva")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        // La columna "impuestos" no existe en la tabla booking.facturacion — no se mapea aquí.

        builder.Property(e => e.Total)
            .HasColumnName("total")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(e => e.ObservacionesFactura)
            .HasColumnName("observaciones_factura")
            .HasColumnType("text");

        builder.Property(e => e.OrigenCanalFactura)
            .HasColumnName("origen_canal_factura")
            .HasMaxLength(100)
            .IsRequired(false);

        // [3] Estado y ciclo de vida
        builder.Property(e => e.Estado)
            .HasColumnName("estado")
            .HasColumnType("char(3)")
            .HasDefaultValue("ABI")
            .IsRequired();

        builder.Property(e => e.EsEliminado)
            .HasColumnName("es_eliminado")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.FechaInhabilitacionUtc)
            .HasColumnName("fecha_inhabilitacion_utc")
            .HasColumnType("timestamptz");

        builder.Property(e => e.MotivoInhabilitacion)
            .HasColumnName("motivo_inhabilitacion")
            .HasMaxLength(255);

        // [4] Auditoría
        builder.Property(e => e.CreadoPorUsuario)
            .HasColumnName("creado_por_usuario")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.FechaRegistroUtc)
            .HasColumnName("fecha_registro_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')")
            .IsRequired();

        builder.Property(e => e.ModificadoPorUsuario)
            .HasColumnName("modificado_por_usuario")
            .HasMaxLength(150);

        builder.Property(e => e.FechaModificacionUtc)
            .HasColumnName("fecha_modificacion_utc")
            .HasColumnType("timestamptz");

        builder.Property(e => e.ModificacionIp)
            .HasColumnName("modificacion_ip")
            .HasMaxLength(45);

        builder.Property(e => e.ServicioOrigen)
            .HasColumnName("servicio_origen")
            .HasMaxLength(100)
            .IsRequired();

        // Restricciones UNIQUE
        builder.HasIndex(e => e.GuidFactura)
            .IsUnique()
            .HasDatabaseName("uq_facturacion_guid");

        builder.HasIndex(e => e.NumeroFactura)
            .IsUnique()
            .HasDatabaseName("uq_facturacion_numero");

        // Relaciones
        builder.HasOne(e => e.Cliente)
            .WithMany()
            .HasForeignKey(e => e.IdCliente)
            .HasConstraintName("fk_facturacion_cliente")
            .OnDelete(DeleteBehavior.NoAction);

        // Filtro global — soft delete
        builder.HasQueryFilter(e => !e.EsEliminado);
    }
}
