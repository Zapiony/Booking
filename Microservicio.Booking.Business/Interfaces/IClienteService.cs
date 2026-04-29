// Microservicio.Booking.Business/Interfaces/IClienteService.cs

using Microservicio.Booking.Business.DTOs.Cliente;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Interfaces;

/// <summary>
/// Contrato del servicio de negocio para clientes.
/// Define las operaciones disponibles para la capa API.
/// No expone DataModels ni entidades — trabaja exclusivamente con DTOs.
/// </summary>
public interface IClienteService
{
    // =========================================================================
    // Consultas
    // =========================================================================

    /// <summary>
    /// Obtiene un cliente por su GUID público.
    /// Lanza NotFoundException si no existe.
    /// </summary>
    Task<ClienteResponse> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por tipo y número de identificación.
    /// Lanza NotFoundException si no existe.
    /// </summary>
    Task<ClienteResponse> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el cliente vinculado a un usuario de autenticación.
    /// Lanza NotFoundException si no existe.
    /// </summary>
    Task<ClienteResponse> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el cliente vinculado a un usuario por su GUID.
    /// Lanza NotFoundException si no existe.
    /// </summary>
    Task<ClienteResponse> ObtenerPorUsuarioGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna una página de clientes aplicando los filtros del request.
    /// </summary>
    Task<DataPagedResult<ClienteResponse>> BuscarAsync(
        ClienteFiltroRequest request,
        CancellationToken cancellationToken = default);

    // =========================================================================
    // Escritura
    // =========================================================================

    /// <summary>
    /// Crea un nuevo cliente aplicando validaciones de negocio.
    /// Lanza ValidationException si los datos son inválidos.
    /// Lanza ValidationException si el correo o identificación ya existen.
    /// Lanza ValidationException si el usuario ya tiene un cliente vinculado.
    /// </summary>
    Task<ClienteResponse> CrearAsync(
        CrearClienteRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// Lanza NotFoundException si el cliente no existe.
    /// Lanza ValidationException si los datos son inválidos.
    /// </summary>
    Task<ClienteResponse> ActualizarAsync(
        ActualizarClienteRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina lógicamente un cliente (es_eliminado = true, estado = INA).
    /// Lanza NotFoundException si el cliente no existe.
    /// </summary>
    Task EliminarLogicoAsync(
        Guid guidCliente,
        string eliminadoPorUsuario,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cambia el estado de un cliente (ACT / INA / SUS).
    /// Lanza NotFoundException si el cliente no existe.
    /// Lanza ValidationException si el estado no es válido.
    /// </summary>
    Task<ClienteResponse> CambiarEstadoAsync(
        Guid guidCliente,
        string nuevoEstado,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);

    // -------------------------------------------------------------------------
    // Verificaciones de disponibilidad — públicas para el registro
    // -------------------------------------------------------------------------

    Task<bool> CorreoDisponibleAsync(
        string correo,
        CancellationToken cancellationToken = default);

    Task<bool> IdentificacionDisponibleAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default);


}