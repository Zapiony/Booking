using Microservicio.Usuarios.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Usuarios.DataAccess.Configurations;

/// <summary>
/// Configuración de EF Core para la tabla booking.usuario_app.
/// Define mapeo de columnas, restricciones, índices y concurrencia optimista.
/// </summary>
public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
    {
        // -------------------------------------------------------------------------
        // Tabla y esquema
        // -------------------------------------------------------------------------
        builder.ToTable("usuario_app", "booking");

        // -------------------------------------------------------------------------
        // [1] Identificación técnica
        // -------------------------------------------------------------------------
        builder.HasKey(u => u.IdUsuario);

        builder.Property(u => u.IdUsuario)
               .HasColumnName("id_usuario")
               .UseIdentityColumn();

        builder.Property(u => u.UsuarioGuid)
               .HasColumnName("usuario_guid")
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.HasIndex(u => u.UsuarioGuid)
               .IsUnique()
               .HasDatabaseName("uq_usuario_app_guid");

        // -------------------------------------------------------------------------
        // [2] Datos funcionales
        // -------------------------------------------------------------------------
        builder.Property(u => u.Username)
               .HasColumnName("username")
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(u => u.Username)
               .IsUnique()
               .HasDatabaseName("uq_usuario_app_username");

        builder.Property(u => u.Correo)
               .HasColumnName("correo")
               .HasMaxLength(120)
               .IsRequired();

        builder.HasIndex(u => u.Correo)
               .IsUnique()
               .HasDatabaseName("uq_usuario_app_correo");

        // -------------------------------------------------------------------------
        // [3] Seguridad
        // -------------------------------------------------------------------------
        builder.Property(u => u.PasswordHash)
               .HasColumnName("password_hash")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(u => u.PasswordSalt)
               .HasColumnName("password_salt")
               .HasMaxLength(250)
               .IsRequired();

        // -------------------------------------------------------------------------
        // [4] Estado y ciclo de vida
        // -------------------------------------------------------------------------
        builder.Property(u => u.EstadoUsuario)
               .HasColumnName("estado_usuario")
               .HasColumnType("CHAR(3)")
               .HasDefaultValue("ACT")
               .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_usuario_app_estado",
            "estado_usuario IN ('ACT', 'INA')"
        ));

        builder.Property(u => u.EsEliminado)
               .HasColumnName("es_eliminado")
               .HasDefaultValue(false)
               .IsRequired();

        builder.Property(u => u.Activo)
               .HasColumnName("activo")
               .HasDefaultValue(true)
               .IsRequired();

        // -------------------------------------------------------------------------
        // [5] Auditoría
        // -------------------------------------------------------------------------
        builder.Property(u => u.FechaRegistroUtc)
               .HasColumnName("fecha_registro_utc")
               .HasColumnType("DATETIME2(0)")
               .HasDefaultValueSql("SYSUTCDATETIME()")
               .IsRequired();

        builder.Property(u => u.CreadoPorUsuario)
               .HasColumnName("creado_por_usuario")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.ModificadoPorUsuario)
               .HasColumnName("modificado_por_usuario")
               .HasMaxLength(100)
               .IsRequired(false);

        builder.Property(u => u.FechaModificacionUtc)
               .HasColumnName("fecha_modificacion_utc")
               .HasColumnType("DATETIME2(0)")
               .IsRequired(false);

        // -------------------------------------------------------------------------
        // [6] Concurrencia optimista — ROWVERSION
        // -------------------------------------------------------------------------
        builder.Property(u => u.RowVersion)
               .HasColumnName("row_version")
               .IsRowVersion()   // EF Core: token de concurrencia optimista
               .IsRequired();

        // -------------------------------------------------------------------------
        // Navegación — UsuarioRol (uno a muchos)
        // -------------------------------------------------------------------------
        builder.HasMany(u => u.UsuariosRoles)
               .WithOne(ur => ur.Usuario)
               .HasForeignKey(ur => ur.IdUsuario)
               .OnDelete(DeleteBehavior.NoAction);
    }
}