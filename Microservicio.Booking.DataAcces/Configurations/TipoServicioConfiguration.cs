using Microservicio.Servicios.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Servicios.DataAccess.Configurations;

/// <summary>
/// Configuración de EF Core para la tabla booking.tipo_servicio.
/// Catálogo cerrado de categorías de proveedor.
/// </summary>
public class TipoServicioConfiguration : IEntityTypeConfiguration<TipoServicioEntity>
{
    public void Configure(EntityTypeBuilder<TipoServicioEntity> builder)
    {
        // -------------------------------------------------------------------------
        // Tabla y esquema
        // -------------------------------------------------------------------------
        builder.ToTable("tipo_servicio", "booking");

        // -------------------------------------------------------------------------
        // [1] Identificación técnica
        // -------------------------------------------------------------------------
        builder.HasKey(ts => ts.IdTipoServicio);

        builder.Property(ts => ts.IdTipoServicio)
               .HasColumnName("id_tipo_servicio")
               .UseIdentityColumn();

        builder.Property(ts => ts.GuidTipoServicio)
               .HasColumnName("guid_tipo_servicio")
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.HasIndex(ts => ts.GuidTipoServicio)
               .IsUnique()
               .HasDatabaseName("uq_tipo_servicio_guid");

        // -------------------------------------------------------------------------
        // [2] Datos funcionales
        // -------------------------------------------------------------------------
        builder.Property(ts => ts.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(60)
               .IsRequired();

        builder.HasIndex(ts => ts.Nombre)
               .IsUnique()
               .HasDatabaseName("uq_tipo_servicio_nombre");

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_tipo_servicio_nombre",
            "nombre IN (N'Vuelos', N'Alojamiento', N'Atracciones', N'Alquiler de Carros')"
        ));

        builder.Property(ts => ts.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(500)
               .IsRequired(false);

        // -------------------------------------------------------------------------
        // [3] Estado y ciclo de vida
        // -------------------------------------------------------------------------
        builder.Property(ts => ts.Estado)
               .HasColumnName("estado")
               .HasColumnType("CHAR(3)")
               .HasDefaultValue("ACT")
               .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_tipo_servicio_estado",
            "estado IN ('ACT', 'INA')"
        ));

        builder.HasIndex(ts => ts.Estado)
               .HasDatabaseName("idx_tipo_servicio_estado");

        builder.Property(ts => ts.EsEliminado)
               .HasColumnName("es_eliminado")
               .HasDefaultValue(false)
               .IsRequired();

        // -------------------------------------------------------------------------
        // [4] Auditoría
        // -------------------------------------------------------------------------
        builder.Property(ts => ts.CreadoPorUsuario)
               .HasColumnName("creado_por_usuario")
               .HasMaxLength(150)
               .IsRequired(false);

        builder.Property(ts => ts.FechaRegistroUtc)
               .HasColumnName("fecha_registro_utc")
               .HasColumnType("DATETIMEOFFSET")
               .HasDefaultValueSql("SYSUTCDATETIME()")
               .IsRequired();

        builder.Property(ts => ts.ModificadoPorUsuario)
               .HasColumnName("modificado_por_usuario")
               .HasMaxLength(150)
               .IsRequired(false);

        builder.Property(ts => ts.FechaModificacionUtc)
               .HasColumnName("fecha_modificacion_utc")
               .HasColumnType("DATETIMEOFFSET")
               .IsRequired(false);

        builder.Property(ts => ts.ModificacionIp)
               .HasColumnName("modificacion_ip")
               .HasMaxLength(45)
               .IsRequired(false);

        builder.Property(ts => ts.ServicioOrigen)
               .HasColumnName("servicio_origen")
               .HasMaxLength(100)
               .IsRequired(false);

        // -------------------------------------------------------------------------
        // [5] Concurrencia optimista — ROWVERSION
        // -------------------------------------------------------------------------
        builder.Property(ts => ts.RowVersion)
               .HasColumnName("row_version")
               .IsRowVersion()
               .IsRequired();

        // -------------------------------------------------------------------------
        // Navegación — Servicio (uno a muchos)
        // -------------------------------------------------------------------------
        builder.HasMany(ts => ts.Servicios)
               .WithOne(s => s.TipoServicio)
               .HasForeignKey(s => s.IdTipoServicio)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
