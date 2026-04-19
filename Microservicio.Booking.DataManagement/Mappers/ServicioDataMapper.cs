using Microservicio.Booking.DataManagement.Models;
using Microservicio.Servicios.DataAccess.Entities;
using Microservicio.Servicios.DataAccess.Queries;

namespace Microservicio.Booking.DataManagement.Mappers;

/// <summary>
/// Conversiones entre entidades/DTOs de DataAccess y modelos de DataManagement.
/// </summary>
public static class ServicioDataMapper
{
    /// <summary>
    /// Usa la navegación <see cref="ServicioEntity.TipoServicio"/> si está cargada; si no, usa <paramref name="tipoServicio"/>.
    /// </summary>
    public static ServicioDataModel AModelo(ServicioEntity entidad, TipoServicioEntity? tipoServicio = null)
    {
        var tipo = entidad.TipoServicio ?? tipoServicio;

        return new ServicioDataModel
        {
            IdServicio = entidad.IdServicio,
            GuidServicio = entidad.GuidServicio,
            IdTipoServicio = entidad.IdTipoServicio,
            GuidTipoServicio = tipo?.GuidTipoServicio ?? Guid.Empty,
            RazonSocial = entidad.RazonSocial,
            NombreComercial = entidad.NombreComercial,
            TipoIdentificacion = entidad.TipoIdentificacion,
            NumeroIdentificacion = entidad.NumeroIdentificacion,
            CorreoContacto = entidad.CorreoContacto,
            TelefonoContacto = entidad.TelefonoContacto,
            Direccion = entidad.Direccion,
            SitioWeb = entidad.SitioWeb,
            LogoUrl = entidad.LogoUrl,
            Estado = entidad.Estado,
            EsEliminado = entidad.EsEliminado,
            CreadoPorUsuario = entidad.CreadoPorUsuario,
            FechaRegistroUtc = entidad.FechaRegistroUtc,
            ModificadoPorUsuario = entidad.ModificadoPorUsuario,
            FechaModificacionUtc = entidad.FechaModificacionUtc,
            ModificacionIp = entidad.ModificacionIp,
            ServicioOrigen = entidad.ServicioOrigen,
            RowVersion = entidad.RowVersion,
            TipoServicioNombre = tipo?.Nombre
        };
    }

    public static ServicioResumenDataModel AResumen(ServicioResumenDto dto) =>
        new(dto.GuidServicio, dto.RazonSocial, dto.NombreComercial, dto.TipoServicioNombre, dto.Estado);

    public static ServicioDataModel AModeloDesdeDetalle(ServicioDetalleDto dto) =>
        new()
        {
            GuidServicio = dto.GuidServicio,
            RazonSocial = dto.RazonSocial,
            NombreComercial = dto.NombreComercial,
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            CorreoContacto = dto.CorreoContacto,
            TelefonoContacto = dto.TelefonoContacto,
            Direccion = dto.Direccion,
            SitioWeb = dto.SitioWeb,
            LogoUrl = dto.LogoUrl,
            Estado = dto.Estado,
            EsEliminado = dto.EsEliminado,
            GuidTipoServicio = dto.GuidTipoServicio,
            TipoServicioNombre = dto.TipoServicioNombre,
            CreadoPorUsuario = dto.CreadoPorUsuario,
            FechaRegistroUtc = dto.FechaRegistroUtc,
            ModificadoPorUsuario = dto.ModificadoPorUsuario,
            FechaModificacionUtc = dto.FechaModificacionUtc
        };

    /// <summary>Crea una entidad nueva para persistencia (sin PK ni token de concurrencia).</summary>
    public static ServicioEntity ANuevaEntidad(ServicioDataModel modelo, int idTipoServicio)
    {
        return new ServicioEntity
        {
            GuidServicio = modelo.GuidServicio != Guid.Empty ? modelo.GuidServicio : Guid.NewGuid(),
            IdTipoServicio = idTipoServicio,
            RazonSocial = modelo.RazonSocial,
            NombreComercial = modelo.NombreComercial,
            TipoIdentificacion = modelo.TipoIdentificacion,
            NumeroIdentificacion = modelo.NumeroIdentificacion,
            CorreoContacto = modelo.CorreoContacto,
            TelefonoContacto = modelo.TelefonoContacto,
            Direccion = modelo.Direccion,
            SitioWeb = modelo.SitioWeb,
            LogoUrl = modelo.LogoUrl,
            Estado = string.IsNullOrWhiteSpace(modelo.Estado) ? "ACT" : modelo.Estado,
            EsEliminado = false,
            CreadoPorUsuario = modelo.CreadoPorUsuario,
            FechaRegistroUtc = modelo.FechaRegistroUtc != default ? modelo.FechaRegistroUtc : DateTimeOffset.UtcNow,
            ModificadoPorUsuario = modelo.ModificadoPorUsuario,
            FechaModificacionUtc = modelo.FechaModificacionUtc,
            ModificacionIp = modelo.ModificacionIp,
            ServicioOrigen = modelo.ServicioOrigen
        };
    }

    public static void AplicarCambios(ServicioEntity destino, ServicioDataModel origen)
    {
        if (origen.IdTipoServicio > 0)
            destino.IdTipoServicio = origen.IdTipoServicio;
        destino.RazonSocial = origen.RazonSocial;
        destino.NombreComercial = origen.NombreComercial;
        destino.TipoIdentificacion = origen.TipoIdentificacion;
        destino.NumeroIdentificacion = origen.NumeroIdentificacion;
        destino.CorreoContacto = origen.CorreoContacto;
        destino.TelefonoContacto = origen.TelefonoContacto;
        destino.Direccion = origen.Direccion;
        destino.SitioWeb = origen.SitioWeb;
        destino.LogoUrl = origen.LogoUrl;
        destino.Estado = origen.Estado;
        destino.ModificadoPorUsuario = origen.ModificadoPorUsuario;
        destino.FechaModificacionUtc = origen.FechaModificacionUtc ?? DateTimeOffset.UtcNow;
        destino.ModificacionIp = origen.ModificacionIp;
        destino.ServicioOrigen = origen.ServicioOrigen;
    }
}
