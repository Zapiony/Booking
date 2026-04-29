// Microservicio.Booking.DataManagement/Services/ClienteDataService.cs

using Microservicio.Booking.DataAccess.Queries;
using Microservicio.Booking.DataAccess.Common;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Mappers;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.DataManagement.Services;

/// <summary>
/// Implementación del servicio de gestión de datos para clientes.
/// Coordina repositorios a través de la UoW, mapea entidades a modelos
/// y confirma cambios. No expone EF Core a capas superiores.
/// </summary>
public class ClienteDataService : IClienteDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // =========================================================================
    // Consultas simples
    // =========================================================================

    public async Task<ClienteDataModel?> ObtenerPorIdAsync(
        int idCliente,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorIdAsync(idCliente, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorGuidAsync(
        Guid guidCliente,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorIdUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorIdUsuarioAsync(idUsuario, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorUsuarioGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorUsuarioGuidAsync(usuarioGuid, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorCorreoAsync(correo, cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ObtenerPorNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorNumeroIdentificacionAsync(
                tipoIdentificacion,
                numeroIdentificacion,
                cancellationToken);

        return entity is null
            ? null
            : ClienteDataMapper.ToDataModel(entity);
    }

    // =========================================================================
    // Consultas paginadas y filtradas
    // =========================================================================

    public async Task<DataPagedResult<ClienteDataModel>> BuscarAsync(
    ClienteFiltroDataModel filtro,
    CancellationToken cancellationToken = default)
    {
        var termino = new[]
        {
        filtro.Nombres,
        filtro.Apellidos,
        filtro.RazonSocial,
        filtro.Correo,
        filtro.NumeroIdentificacion
    }
        .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t)) ?? string.Empty;

        PagedResult<ClienteResumenDto> resultado;

        if (!string.IsNullOrWhiteSpace(termino))
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .BuscarClientesAsync(
                    termino,
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .ListarClientesPorEstadoAsync(
                    filtro.Estado,
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }
        else
        {
            resultado = await _unitOfWork.ClienteQueryRepository
                .ListarClientesAsync(
                    filtro.PaginaActual,
                    filtro.TamanioPagina,
                    cancellationToken);
        }

        return DataPagedResult<ClienteDataModel>.DesdeDal(
            resultado,
            dto => new ClienteDataModel
            {
                GuidCliente = dto.GuidCliente,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                RazonSocial = dto.RazonSocial,
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Estado = dto.Estado
            });
    }

    // =========================================================================
    // Validaciones de unicidad
    // =========================================================================

    public async Task<bool> ExisteCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteCorreoAsync(correo, cancellationToken);
    }

    public async Task<bool> ExisteNumeroIdentificacionAsync(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteNumeroIdentificacionAsync(
                tipoIdentificacion,
                numeroIdentificacion,
                cancellationToken);
    }

    public async Task<bool> ExisteUsuarioVinculadoAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ClienteRepository
            .ExisteUsuarioVinculadoAsync(idUsuario, cancellationToken);
    }

    // =========================================================================
    // Escritura
    // =========================================================================

    public async Task<ClienteDataModel> CrearAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = ClienteDataMapper.ToEntity(model);

        await _unitOfWork.ClienteRepository
            .AgregarAsync(entity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<ClienteDataModel?> ActualizarAsync(
        ClienteDataModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(model.GuidCliente, cancellationToken);

        if (entity is null) return null;

        // Actualiza solo los campos modificables
        entity.Nombres = model.Nombres;
        entity.Apellidos = model.Apellidos;
        entity.RazonSocial = model.RazonSocial;
        entity.TipoIdentificacion = model.TipoIdentificacion;
        entity.NumeroIdentificacion = model.NumeroIdentificacion;
        entity.Correo = model.Correo;
        entity.Telefono = model.Telefono;
        entity.Direccion = model.Direccion;
        entity.Estado = model.Estado;
        entity.ModificadoPorUsuario = model.ModificadoPorUsuario;
        entity.ModificacionIp = model.ModificacionIp;
        entity.ServicioOrigen = model.ServicioOrigen;

        _unitOfWork.ClienteRepository.Actualizar(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClienteDataMapper.ToDataModel(entity);
    }

    public async Task<bool> EliminarLogicoAsync(
        Guid guidCliente,
        string eliminadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        if (entity is null) return false;

        _unitOfWork.ClienteRepository.EliminarLogico(entity);
        entity.ModificadoPorUsuario = eliminadoPorUsuario;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        Guid guidCliente,
        string nuevoEstado,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ClienteRepository
            .ObtenerPorGuidAsync(guidCliente, cancellationToken);

        if (entity is null) return false;

        _unitOfWork.ClienteRepository.CambiarEstado(entity, nuevoEstado);
        entity.ModificadoPorUsuario = modificadoPorUsuario;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}