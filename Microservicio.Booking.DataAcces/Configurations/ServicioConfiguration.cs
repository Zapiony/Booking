using Microservicio.Servicios.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Servicios.DataAccess.Configurations;

/// <summary>
/// Configuración de EF Core para la tabla booking.servicio.
/// Proveedores registrados en la plataforma, vinculados a un tipo de servicio.
/// </summary>
public class ServicioConfiguration : IEntityTypeConfiguration<ServicioEntity>
{
    public void Configure(EntityTypeBuilder<ServicioEntity> builder)
    {
        // -------------------------------------------------------------------------
        // Tabla y esquema
        // -------------------------------------------------------------------------
        builder.ToTable("servicio", "booking");

        // -------------------------------------------------------------------------
        // [1] Identificación técnica
        // -------------------------------------------------------------------------
        builder.HasKey(s => s.IdServicio);

        builder.Property(s => s.IdServicio)
               .HasColumnName("id_servicio")
               .UseIdentityColumn();

        builder.Property(s => s.GuidServicio)
               .HasColumnName("guid_servicio")
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.HasIndex(s => s.GuidServicio)
               .IsUnique()
               .HasDatabaseName("uq_servicio_guid");

        // -------------------------------------------------------------------------
        // [2] Datos funcionales
        // -------------------------------------------------------------------------
        builder.Property(s => s.IdTipoServicio)
               .HasColumnName("id_tipo_servicio")
               .IsRequired();

        builder.HasIndex(s => s.IdTipoServicio)
               .HasDatabaseName("idx_servicio_tipo");

        builder.Property(s => s.RazonSocial)
               .HasColumnName("razon_social")
               .HasMaxLength(200)
               .IsRequired();

        builder.HasIndex(s => s.RazonSocial)
               .HasDatabaseName("idx_servicio_razon_social");

        builder.Property(s => s.NombreComercial)
               .HasColumnName("nombre_comercial")
               .HasMaxLength(200)
               .IsRequired(false);

        builder.Property(s => s.TipoIdentificacion)
               .HasColumnName("tipo_identificacion")
               .HasMaxLength(10)
               .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_servicio_tipo_id",
            "tipo_identificacion IN ('RUC', 'CI', 'PASS', 'EXT')"
        ));

        builder.Property(s => s.NumeroIdentificacion)
               .HasColumnName("numero_identificacion")
               .HasMaxLength(20)
               .IsRequired();

        builder.HasIndex(s => new { s.TipoIdentificacion, s.NumeroIdentificacion })
               .IsUnique()
               .HasDatabaseName("uq_servicio_numero_id");

        builder.Property(s => s.CorreoContacto)
               .HasColumnName("correo_contacto")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(s => s.TelefonoContacto)
               .HasColumnName("telefono_contacto")
               .HasMaxLength(30)
               .IsRequired(false);

        builder.Property(s => s.Direccion)
               .HasColumnName("direccion")
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(s => s.SitioWeb)
               .HasColumnName("sitio_web")
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(s => s.LogoUrl)
               .HasColumnName("logo_url")
               .HasMaxLength(500)
               .IsRequired(false);

        // -------------------------------------------------------------------------
        // [3] Estado y ciclo de vida
        // -------------------------------------------------------------------------
        builder.Property(s => s.Estado)
               .HasColumnName("estado")
               .HasColumnType("CHAR(3)")
               .HasDefaultValue("ACT")
               .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_servicio_estado",
            "estado IN ('ACT', 'INA', 'SUS')"
        ));

        builder.HasIndex(s => s.Estado)
               .HasDatabaseName("idx_servicio_estado");

        builder.Property(s => s.EsEliminado)
               .HasColumnName("es_eliminado")
               .HasDefaultValue(false)
               .IsRequired();

        // -------------------------------------------------------------------------
        // [4] Auditoría
        // -------------------------------------------------------------------------
        builder.Property(s => s.CreadoPorUsuario)
               .HasColumnName("creado_por_usuario")
               .HasMaxLength(150)
               .IsRequired(false);

        builder.Property(s => s.FechaRegistroUtc)
               .HasColumnName("fecha_registro_utc")
               .HasColumnType("DATETIMEOFFSET")
               .HasDefaultValueSql("SYSUTCDATETIME()")
               .IsRequired();

        builder.Property(s => s.ModificadoPorUsuario)
               .HasColumnName("modificado_por_usuario")
               .HasMaxLength(150)
               .IsRequired(false);

        builder.Property(s => s.FechaModificacionUtc)
               .HasColumnName("fecha_modificacion_utc")
               .HasColumnType("DATETIMEOFFSET")
               .IsRequired(false);

        builder.Property(s => s.ModificacionIp)
               .HasColumnName("modificacion_ip")
               .HasMaxLength(45)
               .IsRequired(false);

        builder.Property(s => s.ServicioOrigen)
               .HasColumnName("servicio_origen")
               .HasMaxLength(100)
               .IsRequired(false);

        // -------------------------------------------------------------------------
        // [5] Concurrencia optimista — ROWVERSION
        // -------------------------------------------------------------------------
        builder.Property(s => s.RowVersion)
               .HasColumnName("row_version")
               .IsRowVersion()
               .IsRequired();

        // -------------------------------------------------------------------------
        // Navegación — TipoServicio (muchos a uno)
        // -------------------------------------------------------------------------
        builder.HasOne(s => s.TipoServicio)
               .WithMany(ts => ts.Servicios)
               .HasForeignKey(s => s.IdTipoServicio)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("fk_servicio_tipo_servicio");
    }
}
