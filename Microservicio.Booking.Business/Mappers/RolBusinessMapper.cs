using Microservicio.Booking.Business.DTOs.Rol;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

/// <summary>
/// Mapper de la capa de negocio para roles.
/// Transforma entre RolDataModel (Capa 2) y DTOs de Rol (Capa 3).
/// </summary>
public static class RolBusinessMapper
{
    // -------------------------------------------------------------------------
    // DataModel → Response
    // -------------------------------------------------------------------------

    public static RolResponse ToResponse(RolDataModel model)
    {
        return new RolResponse
        {
            RolGuid = model.RolGuid,
            NombreRol = model.NombreRol,
            DescripcionRol = model.DescripcionRol,
            EstadoRol = model.EstadoRol,
            Activo = model.Activo
        };
    }

    // -------------------------------------------------------------------------
    // CrearRolRequest → DataModel
    // -------------------------------------------------------------------------

    public static RolDataModel ToDataModel(CrearRolRequest request)
    {
        return new RolDataModel
        {
            NombreRol = request.NombreRol.Trim().ToUpper(),
            DescripcionRol = request.DescripcionRol?.Trim(),
            EstadoRol = "ACT",
            Activo = true,
            EsEliminado = false,
            CreadoPorUsuario = request.CreadoPorUsuario
        };
    }
}