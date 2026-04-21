using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Interfaces;

/// <summary>
/// Contrato del servicio de gestión de datos para clientes.
/// Define operaciones de persistencia de alto nivel para ser
/// consumidas por la capa de lógica de negocio.
/// No expone entidades de EF Core — trabaja exclusivamente con DataModels.
/// </summary>
public interface IClienteDataService
{
    // Consultas simples

    /// <summary>
    /// Obtiene un cliente por su ID interno.
    /// Retorna null si no existe o está eliminado.
    /// </summary>
    Task<ClienteDataModel?> ObtenerPorIdAsync(
        int idCliente,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por su GUID público (el que expone la API).
    /// Retorna null si no existe o está eliminado.
    /// </summary>
    Task<ClienteDataModel?> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el cliente vinculado a un usuario de autenticación.
    /// Relación 1:1 con usuario_app.
    /// </summary>
    Task<ClienteDataModel?> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por su correo electrónico.
    /// Retorna null si no existe o está eliminado.
    /// </summary>
    Task<ClienteDataModel?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente por tipo y número de identificación.
    /// Retorna null si no existe o está eliminado.
    /// </summary>
    Task<ClienteDataModel?> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default);

    // Consultas paginadas y filtradas

    /// <summary>
    /// Retorna una página de clientes aplicando los filtros del modelo.
    /// </summary>
    Task<DataPagedResult<ClienteDataModel>> BuscarAsync(
        ClienteFiltroDataModel filtro,
        CancellationToken cancellationToken = default);

    // Validaciones de unicidad

    /// <summary>
    /// Verifica si ya existe un cliente con ese correo.
    /// </summary>
    Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si ya existe un cliente con ese número de identificación.
    /// </summary>
    Task<bool> ExisteNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si ya existe un cliente vinculado a ese usuario.
    /// Garantiza la relación 1:1 con usuario_app.
    /// </summary>
    Task<bool> ExisteUsuarioVinculadoAsync(
        int idUsuario,
        CancellationToken cancellationToken = default);

    // Escritura

    /// <summary>
    /// Crea un nuevo cliente y confirma los cambios.
    /// Retorna el modelo con los valores generados por PostgreSQL.
    /// </summary>
    Task<ClienteDataModel> CrearAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos de un cliente existente y confirma los cambios.
    /// Retorna null si el cliente no existe.
    /// </summary>
    Task<ClienteDataModel?> ActualizarAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft delete — marca el cliente como eliminado y lo inactiva.
    /// Retorna false si el cliente no existe.
    /// </summary>
    Task<bool> EliminarLogicoAsync(
        Guid guidCliente,
        string eliminadoPorUsuario,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cambia el estado del cliente (ACT / INA / SUS).
    /// Retorna false si el cliente no existe.
    /// </summary>
    Task<bool> CambiarEstadoAsync(
        Guid guidCliente,
        string nuevoEstado,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default);
}