// Microservicio.Booking.Business/Services/ClienteService.cs

using Microservicio.Booking.Business.DTOs.Cliente;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Implementación del servicio de negocio para clientes.
/// Aplica validaciones, reglas de negocio y coordina
/// operaciones con la capa de gestión de datos.
/// No accede directamente a repositorios ni a EF Core.
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IClienteDataService _clienteDataService;

    public ClienteService(IClienteDataService clienteDataService)
    {
        _clienteDataService = clienteDataService;
    }

    // =========================================================================
    // Consultas
    // =========================================================================

    public async Task<ClienteResponse> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        var model = await _clienteDataService
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        if (model is null)
            throw new NotFoundException(
                $"No se encontró el cliente con GUID '{guidCliente}'.");

        return ClienteBusinessMapper.ToResponse(model);
    }

    public async Task<ClienteResponse> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        var model = await _clienteDataService
            .ObtenerPorNumeroIdentificacionAsync(
                tipoIdentificacion,
                numeroIdentificacion,
                cancellationToken);

        if (model is null)
            throw new NotFoundException(
                $"No se encontró el cliente con identificación '{numeroIdentificacion}'.");

        return ClienteBusinessMapper.ToResponse(model);
    }

    public async Task<ClienteResponse> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        var model = await _clienteDataService
            .ObtenerPorIdUsuarioAsync(idUsuario, cancellationToken);

        if (model is null)
            throw new NotFoundException(
                $"No se encontró un cliente vinculado al usuario '{idUsuario}'.");

        return ClienteBusinessMapper.ToResponse(model);
    }

    public async Task<ClienteResponse> ObtenerPorUsuarioGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var model = await _clienteDataService
            .ObtenerPorUsuarioGuidAsync(usuarioGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException(
                $"No se encontró un cliente vinculado al usuario con GUID '{usuarioGuid}'.");

        return ClienteBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<ClienteResponse>> BuscarAsync(
    ClienteFiltroRequest request,
    CancellationToken cancellationToken = default)
    {
        var filtro = ClienteBusinessMapper.ToFiltroDataModel(request);

        var resultado = await _clienteDataService
            .BuscarAsync(filtro, cancellationToken);

        // ✅ CORRECTO — constructor con propiedades en español
        var items = ClienteBusinessMapper.ToResponseList(resultado.Items);

        return new DataPagedResult<ClienteResponse>(
            items,
            resultado.TotalRegistros,
            resultado.PaginaActual,
            resultado.TamanoPagina);
    }

    // =========================================================================
    // Escritura
    // =========================================================================

    public async Task<ClienteResponse> CrearAsync(
        CrearClienteRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validaciones de negocio
        var errores = ClienteValidator.ValidarCreacion(request);
        if (errores.Any())
            throw new ValidationException(
                "La solicitud de creación de cliente es inválida.", errores);

        // 2. Verificar que el usuario no tenga ya un cliente vinculado
        var usuarioVinculado = await _clienteDataService
            .ExisteUsuarioVinculadoAsync(request.IdUsuario, cancellationToken);

        if (usuarioVinculado)
            throw new ValidationException(
                $"El usuario '{request.IdUsuario}' ya tiene un cliente vinculado.");

        // 3. Verificar unicidad de correo
        var correoExiste = await _clienteDataService
            .ExisteCorreoAsync(request.Correo.ToLower().Trim(), cancellationToken);

        if (correoExiste)
            throw new ValidationException(
                $"Ya existe un cliente registrado con el correo '{request.Correo}'.");

        // 4. Verificar unicidad de identificación
        var identificacionExiste = await _clienteDataService
            .ExisteNumeroIdentificacionAsync(
                request.TipoIdentificacion.ToUpper(),
                request.NumeroIdentificacion,
                cancellationToken);

        if (identificacionExiste)
            throw new ValidationException(
                $"Ya existe un cliente con la identificación '{request.NumeroIdentificacion}'.");

        // 5. Mapear y persistir
        var dataModel = ClienteBusinessMapper.ToDataModel(request);
        var creado = await _clienteDataService.CrearAsync(dataModel, cancellationToken);

        return ClienteBusinessMapper.ToResponse(creado);
    }

    public async Task<ClienteResponse> ActualizarAsync(
        ActualizarClienteRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validaciones de negocio
        var errores = ClienteValidator.ValidarActualizacion(request);
        if (errores.Any())
            throw new ValidationException(
                "La solicitud de actualización de cliente es inválida.", errores);

        // 2. Verificar que el cliente exista
        var existente = await _clienteDataService
            .ObtenerPorGuidAsync(request.GuidCliente, cancellationToken);

        if (existente is null)
            throw new NotFoundException(
                $"No se encontró el cliente con GUID '{request.GuidCliente}'.");

        // 3. Verificar unicidad de correo — solo si cambió
        if (!string.Equals(existente.Correo, request.Correo.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            var correoExiste = await _clienteDataService
                .ExisteCorreoAsync(request.Correo.ToLower().Trim(), cancellationToken);

            if (correoExiste)
                throw new ValidationException(
                    $"Ya existe un cliente registrado con el correo '{request.Correo}'.");
        }

        // 4. Verificar unicidad de identificación — solo si cambió
        if (existente.TipoIdentificacion != request.TipoIdentificacion.ToUpper() ||
            existente.NumeroIdentificacion != request.NumeroIdentificacion)
        {
            var identificacionExiste = await _clienteDataService
                .ExisteNumeroIdentificacionAsync(
                    request.TipoIdentificacion.ToUpper(),
                    request.NumeroIdentificacion,
                    cancellationToken);

            if (identificacionExiste)
                throw new ValidationException(
                    $"Ya existe un cliente con la identificación '{request.NumeroIdentificacion}'.");
        }

        // 5. Mapear y persistir
        var dataModel = ClienteBusinessMapper.ToDataModel(request);
        var actualizado = await _clienteDataService.ActualizarAsync(dataModel, cancellationToken);

        if (actualizado is null)
            throw new NotFoundException(
                "No se pudo actualizar el cliente porque no fue encontrado.");

        return ClienteBusinessMapper.ToResponse(actualizado);
    }

    public async Task EliminarLogicoAsync(
        Guid guidCliente,
        string eliminadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var eliminado = await _clienteDataService
            .EliminarLogicoAsync(guidCliente, eliminadoPorUsuario, cancellationToken);

        if (!eliminado)
            throw new NotFoundException(
                $"No se encontró el cliente con GUID '{guidCliente}' para eliminación.");
    }

    public async Task<ClienteResponse> CambiarEstadoAsync(
        Guid guidCliente,
        string nuevoEstado,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar estado
        var errores = ClienteValidator.ValidarCambioEstado(nuevoEstado);
        if (errores.Any())
            throw new ValidationException(
                "El estado indicado no es válido.", errores);

        // 2. Cambiar estado
        var cambiado = await _clienteDataService
            .CambiarEstadoAsync(
                guidCliente,
                nuevoEstado.ToUpper(),
                modificadoPorUsuario,
                cancellationToken);

        if (!cambiado)
            throw new NotFoundException(
                $"No se encontró el cliente con GUID '{guidCliente}'.");

        // 3. Retornar cliente actualizado
        var model = await _clienteDataService
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        return ClienteBusinessMapper.ToResponse(model!);
    }

    public async Task<bool> CorreoDisponibleAsync(
    string correo,
    CancellationToken cancellationToken = default)
    {
        return !await _clienteDataService.ExisteCorreoAsync(correo, cancellationToken);
    }

    public async Task<bool> IdentificacionDisponibleAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return !await _clienteDataService.ExisteNumeroIdentificacionAsync(
            tipoIdentificacion,
            numeroIdentificacion,
            cancellationToken);
    }

}