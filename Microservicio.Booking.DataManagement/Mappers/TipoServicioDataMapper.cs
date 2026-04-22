using Microservicio.Booking.DataManagement.Models;
using Microservicio.Booking.DataAccess.Entities;

namespace Microservicio.Booking.DataManagement.Mappers;

/// <summary>
/// Conversiones entre <see cref="TipoServicioEntity"/> y <see cref="TipoServicioDataModel"/>.
/// </summary>
public static class TipoServicioDataMapper
{
    public static TipoServicioDataModel AModelo(TipoServicioEntity entidad) =>
        new()
        {
            IdTipoServicio = entidad.IdTipoServicio,
            GuidTipoServicio = entidad.GuidTipoServicio,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion,
            Estado = entidad.Estado,
            EsEliminado = entidad.EsEliminado,
            CreadoPorUsuario = entidad.CreadoPorUsuario,
            FechaRegistroUtc = entidad.FechaRegistroUtc,
            ModificadoPorUsuario = entidad.ModificadoPorUsuario,
            FechaModificacionUtc = entidad.FechaModificacionUtc,
            ModificacionIp = entidad.ModificacionIp,
            ServicioOrigen = entidad.ServicioOrigen
        };

    public static TipoServicioEntity ANuevaEntidad(TipoServicioDataModel modelo) =>
        new()
        {
            GuidTipoServicio = modelo.GuidTipoServicio != Guid.Empty ? modelo.GuidTipoServicio : Guid.NewGuid(),
            Nombre = modelo.Nombre,
            Descripcion = modelo.Descripcion,
            Estado = string.IsNullOrWhiteSpace(modelo.Estado) ? "ACT" : modelo.Estado,
            EsEliminado = false,
            CreadoPorUsuario = modelo.CreadoPorUsuario,
            FechaRegistroUtc = modelo.FechaRegistroUtc != default ? modelo.FechaRegistroUtc : DateTimeOffset.UtcNow,
            ModificadoPorUsuario = modelo.ModificadoPorUsuario,
            FechaModificacionUtc = modelo.FechaModificacionUtc,
            ModificacionIp = modelo.ModificacionIp,
            ServicioOrigen = modelo.ServicioOrigen
        };

    public static void AplicarCambios(TipoServicioEntity destino, TipoServicioDataModel origen)
    {
        destino.Nombre = origen.Nombre;
        destino.Descripcion = origen.Descripcion;
        destino.Estado = origen.Estado;
        destino.ModificadoPorUsuario = origen.ModificadoPorUsuario;
        destino.FechaModificacionUtc = origen.FechaModificacionUtc ?? DateTimeOffset.UtcNow;
        destino.ModificacionIp = origen.ModificacionIp;
        destino.ServicioOrigen = origen.ServicioOrigen;
    }
}
