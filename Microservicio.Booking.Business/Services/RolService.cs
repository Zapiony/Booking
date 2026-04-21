using Microservicio.Booking.Business.DTOs.Rol;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Servicio de negocio para la gestión de roles y asignaciones usuario-rol.
/// Orquesta validaciones, reglas de dominio y llamadas a la capa 2.
/// No accede a EF Core ni a repositorios directamente.
/// </summary>
public class RolService : IRolService
{
    private readonly IRolDataService _rolDataService;
    private readonly IUsuarioDataService _usuarioDataService;

    public RolService(
        IRolDataService rolDataService,
        IUsuarioDataService usuarioDataService)
    {
        _rolDataService = rolDataService;
        _usuarioDataService = usuarioDataService;
    }

    // -------------------------------------------------------------------------
    // Catálogo de roles
    // -------------------------------------------------------------------------

    public async Task<IReadOnlyList<RolResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = await _rolDataService.ObtenerTodosLosRolesAsync(cancellationToken);
        return roles.Select(RolBusinessMapper.ToResponse).ToList();
    }

    public async Task<RolResponse?> ObtenerPorGuidAsync(
        Guid rolGuid,
        CancellationToken cancellationToken = default)
    {
        var model = await _rolDataService.ObtenerRolPorGuidAsync(rolGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException("Rol", rolGuid);

        return RolBusinessMapper.ToResponse(model);
    }

    public async Task<RolResponse> CrearAsync(
        CrearRolRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar campos
        RolValidator.ValidarCrear(request);

        // 2. Verificar unicidad del nombre
        if (await _rolDataService.ExisteNombreRolAsync(request.NombreRol, cancellationToken))
            throw new ValidationException($"El rol '{request.NombreRol}' ya existe en el catálogo.");

        // 3. Mapear y persistir
        var dataModel = RolBusinessMapper.ToDataModel(request);
        var creado = await _rolDataService.CrearRolAsync(dataModel, cancellationToken);

        return RolBusinessMapper.ToResponse(creado);
    }

    public async Task EliminarLogicoAsync(
        Guid rolGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        // Verificar que el rol existe
        var rol = await _rolDataService.ObtenerRolPorGuidAsync(rolGuid, cancellationToken)
            ?? throw new NotFoundException("Rol", rolGuid);

        var eliminado = await _rolDataService
            .EliminarLogicoRolAsync(rol.IdRol, modificadoPorUsuario, cancellationToken);

        if (!eliminado)
            throw new NotFoundException("Rol", rolGuid);
    }

    // -------------------------------------------------------------------------
    // Asignaciones usuario-rol
    // -------------------------------------------------------------------------

    public async Task<IReadOnlyList<RolResponse>> ObtenerRolesDeUsuarioAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", usuarioGuid);

        var roles = await _rolDataService
            .ObtenerRolesDeUsuarioAsync(usuario.IdUsuario, cancellationToken);

        return roles.Select(RolBusinessMapper.ToResponse).ToList();
    }

    public async Task<IReadOnlyList<UsuarioRolResponse>> ObtenerAsignacionesDeUsuarioAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", usuarioGuid);

        var asignaciones = await _rolDataService
            .ObtenerAsignacionesDeUsuarioAsync(usuario.IdUsuario, cancellationToken);

        return asignaciones
            .Select(RolBusinessMapper.ToUsuarioRolResponse)
            .ToList();
    }

    public async Task AsignarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar campos
        RolValidator.ValidarAsignacion(request);

        // 2. Verificar que el usuario existe
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", request.UsuarioGuid);

        // 3. Verificar que el rol existe
        var rol = await _rolDataService
            .ObtenerRolPorGuidAsync(request.RolGuid, cancellationToken)
            ?? throw new NotFoundException("Rol", request.RolGuid);

        // 4. Verificar que no tenga ya ese rol asignado
        if (await _rolDataService.UsuarioTieneRolAsync(usuario.IdUsuario, rol.IdRol, cancellationToken))
            throw new ValidationException(
                $"El usuario ya tiene asignado el rol '{rol.NombreRol}'.");

        // 5. Asignar
        await _rolDataService.AsignarRolAsync(
            usuario.IdUsuario,
            rol.IdRol,
            request.EjecutadoPorUsuario,
            cancellationToken);
    }

    public async Task RevocarRolAsync(
        AsignarRolRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar campos
        RolValidator.ValidarAsignacion(request);

        // 2. Verificar que el usuario existe
        var usuario = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", request.UsuarioGuid);

        // 3. Verificar que el rol existe
        var rol = await _rolDataService
            .ObtenerRolPorGuidAsync(request.RolGuid, cancellationToken)
            ?? throw new NotFoundException("Rol", request.RolGuid);

        // 4. Verificar que la asignación existe antes de revocar
        if (!await _rolDataService.UsuarioTieneRolAsync(usuario.IdUsuario, rol.IdRol, cancellationToken))
            throw new ValidationException(
                $"El usuario no tiene asignado el rol '{rol.NombreRol}'.");

        // 5. Revocar
        var revocado = await _rolDataService.RevocarRolAsync(
            usuario.IdUsuario,
            rol.IdRol,
            request.EjecutadoPorUsuario,
            cancellationToken);

        if (!revocado)
            throw new NotFoundException("Asignación de rol", $"{request.UsuarioGuid}/{request.RolGuid}");
    }
}