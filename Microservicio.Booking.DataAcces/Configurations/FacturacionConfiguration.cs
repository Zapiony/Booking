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
        builder.HasKey(e => e.IdFacturacion);

        builder.Property(e => e.IdFacturacion)
            .HasColumnName("id_facturacion")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.GuidFacturacion)
            .HasColumnName("guid_facturacion")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        // [2] Datos funcionales
        builder.Property(e => e.IdCliente)
            .HasColumnName("id_cliente")
            .IsRequired();

        builder.Property(e => e.IdReserva)
            .HasColumnName("id_reserva")
            .IsRequired();

        builder.Property(e => e.NumeroFactura)
            .HasColumnName("numero_factura")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Impuestos)
            .HasColumnName("impuestos")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Total)
            .HasColumnName("total")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // [3] Estado y ciclo de vida
        builder.Property(e => e.Estado)
            .HasColumnName("estado")
            .HasColumnType("char(3)")
            .HasDefaultValue("ACT")
            .IsRequired();

        builder.Property(e => e.EsEliminado)
            .HasColumnName("es_eliminado")
            .HasDefaultValue(false)
            .IsRequired();

        // [4] Auditoría
        builder.Property(e => e.CreadoPorUsuario)
            .HasColumnName("creado_por_usuario")
            .HasMaxLength(150);

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
            .HasMaxLength(100);

        // Restricciones UNIQUE
        builder.HasIndex(e => e.GuidFacturacion)
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
