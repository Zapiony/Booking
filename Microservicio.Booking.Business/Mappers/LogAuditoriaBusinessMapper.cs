using Microservicio.Booking.Business.DTOs.LogAuditoria;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

public static class LogAuditoriaBusinessMapper
{
    public static LogAuditoriaDataModel ToDataModel(CrearLogAuditoriaRequest request)
    {
        return new LogAuditoriaDataModel
        {
            TablaAfectada = request.TablaAfectada,
            Operacion = request.Operacion.ToUpperInvariant(),
            IdRegistro = request.IdRegistro,
            DatosAnteriores = request.DatosAnteriores,
            DatosNuevos = request.DatosNuevos,
            
            FechaUtc = DateTimeOffset.UtcNow,
            CreadoPorUsuario = request.CreadoPorUsuario,
            Ip = request.Ip,
            ServicioOrigen = request.ServicioOrigen,
            EquipoOrigen = request.EquipoOrigen
        };
    }

    public static LogAuditoriaResponse ToResponse(LogAuditoriaDataModel model)
    {
        return new LogAuditoriaResponse
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
            EquipoOrigen = model.EquipoOrigen
        };
    }
}
