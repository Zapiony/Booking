using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Mappers;

/// <summary>
/// Mapeador estático entre ClienteEntity (capa de acceso a datos)
/// y ClienteDataModel (capa de gestión de datos).
/// Evita que las capas superiores dependan directamente de EF Core.
/// </summary>
public static class ClienteDataMapper
{
    // Entity → DataModel

    /// <summary>
    /// Convierte una entidad de EF Core en un modelo de datos
    /// para ser consumido por capas superiores.
    /// </summary>
    public static ClienteDataModel ToDataModel(ClienteEntity entity)
    {
        return new ClienteDataModel
        {
            IdCliente = entity.IdCliente,
            GuidCliente = entity.GuidCliente,
            IdUsuario = entity.IdUsuario,
            Nombres = entity.Nombres,
            Apellidos = entity.Apellidos,
            RazonSocial = entity.RazonSocial,
            TipoIdentificacion = entity.TipoIdentificacion,
            NumeroIdentificacion = entity.NumeroIdentificacion,
            Correo = entity.Correo,
            Telefono = entity.Telefono,
            Direccion = entity.Direccion,
            Estado = entity.Estado,
            EsEliminado = entity.EsEliminado,
            CreadoPorUsuario = entity.CreadoPorUsuario,
            FechaRegistroUtc = entity.FechaRegistroUtc,
            ModificadoPorUsuario = entity.ModificadoPorUsuario,
            FechaModificacionUtc = entity.FechaModificacionUtc,
            ModificacionIp = entity.ModificacionIp,
            ServicioOrigen = entity.ServicioOrigen
        };
    }

    // DataModel → Entity

    /// <summary>
    /// Convierte un modelo de datos en una entidad de EF Core
    /// para operaciones de escritura en la base de datos.
    /// </summary>
    public static ClienteEntity ToEntity(ClienteDataModel model)
    {
        return new ClienteEntity
        {
            IdCliente = model.IdCliente,
            GuidCliente = model.GuidCliente,
            IdUsuario = model.IdUsuario,
            Nombres = model.Nombres,
            Apellidos = model.Apellidos,
            RazonSocial = model.RazonSocial,
            TipoIdentificacion = model.TipoIdentificacion,
            NumeroIdentificacion = model.NumeroIdentificacion,
            Correo = model.Correo,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            Estado = model.Estado,
            EsEliminado = model.EsEliminado,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,
            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc,
            ModificacionIp = model.ModificacionIp,
            ServicioOrigen = model.ServicioOrigen
        };
    }

    // Colecciones

    /// <summary>
    /// Convierte una colección de entidades en una lista de modelos de datos.
    /// </summary>
    public static IReadOnlyList<ClienteDataModel> ToDataModelList(
        IEnumerable<ClienteEntity> entities)
    {
        return entities
            .Select(ToDataModel)
            .ToList();
    }
}