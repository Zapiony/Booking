using Microservicio.Booking.DataAccess.Entities;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Mappers;

public static class LogAuditoriaDataMapper
{
    public static LogAuditoriaDataModel ToDataModel(LogAuditoriaEntity entity)
    {
        return new LogAuditoriaDataModel
        {
            IdLog = entity.IdLog,
            TablaAfectada = entity.TablaAfectada,
            Operacion = entity.Operacion,
            IdRegistro = entity.IdRegistro,
            DatosAnteriores = entity.DatosAnteriores,
            DatosNuevos = entity.DatosNuevos,
            CreadoPorUsuario = entity.CreadoPorUsuario,
            FechaUtc = entity.FechaUtc,
            Ip = entity.Ip,
            ServicioOrigen = entity.ServicioOrigen,
            EquipoOrigen = entity.EquipoOrigen,
            EsEliminadoLog = entity.EsEliminadoLog
        };
    }

    public static LogAuditoriaEntity ToEntity(LogAuditoriaDataModel model)
    {
        return new LogAuditoriaEntity
        {
            IdLog = model.IdLog,
            TablaAfectada = model.TablaAfectada,
            Operacion = model.Operacion,
            IdRegistro = model.IdRegistro,
            DatosAnteriores = model.DatosAnteriores,
            DatosNuevos = model.DatosNuevos,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaUtc = model.FechaUtc,
            Ip = model.Ip,
            ServicioOrigen = model.ServicioOrigen,
            EquipoOrigen = model.EquipoOrigen,
            EsEliminadoLog = model.EsEliminadoLog
        };
    }

    public static IReadOnlyList<LogAuditoriaDataModel> ToDataModelList(IEnumerable<LogAuditoriaEntity> entities)
    {
        return entities.Select(ToDataModel).ToList();
    }
}
