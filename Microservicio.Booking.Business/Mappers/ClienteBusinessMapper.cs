// Microservicio.Booking.Business/Mappers/ClienteBusinessMapper.cs

using Microservicio.Booking.Business.DTOs.Cliente;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Mappers;

/// <summary>
/// Mapeador estático entre DTOs de Business y DataModels de DataManagement.
/// La capa de negocio nunca expone DataModels hacia arriba
/// ni recibe entidades desde abajo.
/// </summary>
public static class ClienteBusinessMapper
{
    // =========================================================================
    // Request → DataModel
    // =========================================================================

    /// <summary>
    /// Convierte un request de creación en un DataModel
    /// para enviarlo a la capa de gestión de datos.
    /// </summary>
    public static ClienteDataModel ToDataModel(CrearClienteRequest request)
    {
        return new ClienteDataModel
        {
            IdUsuario = request.IdUsuario,
            Nombres = request.Nombres?.Trim(),
            Apellidos = request.Apellidos?.Trim(),
            RazonSocial = request.RazonSocial?.Trim(),
            TipoIdentificacion = request.TipoIdentificacion.ToUpper().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion.Trim(),
            Correo = request.Correo.ToLower().Trim(),
            Telefono = request.Telefono?.Trim(),
            Direccion = null,
            Estado = "ACT",
            EsEliminado = false,
            CreadoPorUsuario = request.CreadoPorUsuario,
            FechaRegistroUtc = DateTimeOffset.UtcNow,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    /// <summary>
    /// Convierte un request de actualización en un DataModel
    /// para enviarlo a la capa de gestión de datos.
    /// </summary>
    public static ClienteDataModel ToDataModel(ActualizarClienteRequest request)
    {
        return new ClienteDataModel
        {
            GuidCliente = request.GuidCliente,
            Nombres = request.Nombres?.Trim(),
            Apellidos = request.Apellidos?.Trim(),
            RazonSocial = request.RazonSocial?.Trim(),
            TipoIdentificacion = request.TipoIdentificacion.ToUpper().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion.Trim(),
            Correo = request.Correo.ToLower().Trim(),
            Telefono = request.Telefono?.Trim(),
            Direccion = request.Direccion?.Trim(),
            Estado = request.Estado.ToUpper().Trim(),
            ModificadoPorUsuario = request.ModificadoPorUsuario,
            FechaModificacionUtc = DateTimeOffset.UtcNow,
            ModificacionIp = request.ModificacionIp,
            ServicioOrigen = request.ServicioOrigen
        };
    }

    /// <summary>
    /// Convierte un request de filtro en un DataModel de filtro
    /// para enviarlo a la capa de gestión de datos.
    /// </summary>
    public static ClienteFiltroDataModel ToFiltroDataModel(ClienteFiltroRequest request)
    {
        return new ClienteFiltroDataModel
        {
            Nombres = request.Nombres?.Trim(),
            Apellidos = request.Apellidos?.Trim(),
            RazonSocial = request.RazonSocial?.Trim(),
            Correo = request.Correo?.ToLower().Trim(),
            TipoIdentificacion = request.TipoIdentificacion?.ToUpper().Trim(),
            NumeroIdentificacion = request.NumeroIdentificacion?.Trim(),
            Estado = request.Estado?.ToUpper().Trim(),
            PaginaActual = request.PaginaActual,
            TamanioPagina = request.TamanioPagina
        };
    }

    // =========================================================================
    // DataModel → Response
    // =========================================================================

    /// <summary>
    /// Convierte un DataModel en un DTO de respuesta
    /// para enviarlo a la API.
    /// </summary>
    public static ClienteResponse ToResponse(ClienteDataModel model)
    {
        return new ClienteResponse
        {
            GuidCliente = model.GuidCliente,
            Nombres = model.Nombres,
            Apellidos = model.Apellidos,
            RazonSocial = model.RazonSocial,
            TipoIdentificacion = model.TipoIdentificacion,
            NumeroIdentificacion = model.NumeroIdentificacion,
            Correo = model.Correo,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            Estado = model.Estado,
            CreadoPorUsuario = model.CreadoPorUsuario,
            FechaRegistroUtc = model.FechaRegistroUtc,
            ModificadoPorUsuario = model.ModificadoPorUsuario,
            FechaModificacionUtc = model.FechaModificacionUtc
        };
    }

    // =========================================================================
    // Colecciones
    // =========================================================================

    /// <summary>
    /// Convierte una colección de DataModels en una lista de responses.
    /// </summary>
    public static IReadOnlyList<ClienteResponse> ToResponseList(
        IEnumerable<ClienteDataModel> models)
    {
        return models
            .Select(ToResponse)
            .ToList();
    }
}