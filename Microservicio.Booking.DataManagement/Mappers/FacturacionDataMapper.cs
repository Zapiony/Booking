using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Mappers;

/// <summary>
/// Mapeador estático entre FacturacionEntity (capa de acceso a datos)
/// y FacturacionDataModel (capa de gestión de datos).
/// Evita que las capas superiores dependan directamente de EF Core.
/// </summary>
public static class FacturacionDataMapper
{
    // -------------------------------------------------------------------------
    // Entity → DataModel
    // -------------------------------------------------------------------------

    /// <summary>
    /// Convierte una entidad de EF Core en un modelo de datos
    /// para ser consumido por capas superiores.
    /// </summary>
    public static FacturacionDataModel ToDataModel(FacturacionEntity entity)
    {
        return new FacturacionDataModel
        {
            IdFactura = entity.IdFactura,
            GuidFactura = entity.GuidFactura,
            IdCliente = entity.IdCliente,
            IdServicio = entity.IdServicio,
            NumeroFactura = entity.NumeroFactura,
            FechaEmision = entity.FechaEmision,
            Subtotal = entity.Subtotal,
            ValorIva = entity.ValorIva,
            Total = entity.Total,
            ObservacionesFactura = entity.ObservacionesFactura,
            OrigenCanalFactura = entity.OrigenCanalFactura,
            Estado = entity.Estado,
            EsEliminado = entity.EsEliminado,
            FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
            MotivoInhabilitacion = entity.MotivoInhabilitacion,
            CreadoPorUsuario = entity.CreadoPorUsuario,
            FechaRegistroUtc = entity.FechaRegistroUtc,
            ModificadoPorUsuario = entity.ModificadoPorUsuario,
            FechaModificacionUtc = entity.FechaModificacionUtc,
            ModificacionIp = entity.ModificacionIp,
            ServicioOrigen = entity.ServicioOrigen
        };
    }

    // -------------------------------------------------------------------------
    // DataModel → Entity
    // -------------------------------------------------------------------------

    /// <summary>
    /// Convierte un modelo de datos en una entidad de EF Core
    /// para operaciones de escritura en la base de datos.
    /// </summary>
    public static FacturacionEntity ToEntity(FacturacionDataModel model)
    {
        return new FacturacionEntity
        {
            IdFactura = model.IdFactura,
            GuidFactura = model.GuidFactura,
            IdCliente = model.IdCliente,
            IdServicio = model.IdServicio,
            NumeroFactura = model.NumeroFactura,
            FechaEmision = model.FechaEmision,
            Subtotal = model.Subtotal,
            ValorIva = model.ValorIva,
            Total = model.Total,
            ObservacionesFactura = model.ObservacionesFactura,
            OrigenCanalFactura = model.OrigenCanalFactura,
            Estado = model.Estado,
            EsEliminado = model.EsEliminado,
            FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
            MotivoInhabilitacion = model.MotivoInhabilitacion,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,
            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc,
            ModificacionIp = model.ModificacionIp,
            ServicioOrigen = model.ServicioOrigen
        };
    }

    // -------------------------------------------------------------------------
    // Colecciones
    // -------------------------------------------------------------------------

    /// <summary>
    /// Convierte una colección de entidades en una lista de modelos de datos.
    /// </summary>
    public static IReadOnlyList<FacturacionDataModel> ToDataModelList(IEnumerable<FacturacionEntity> entities)
    {
        return entities.Select(ToDataModel).ToList();
    }
}
