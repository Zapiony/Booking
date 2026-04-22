using Microservicio.Booking.Business.DTOs.Usuario;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Servicio de negocio para la gestión de usuarios.
/// Orquesta validaciones, reglas de dominio y llamadas a la capa 2.
/// No accede a EF Core ni a repositorios directamente.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioDataService _usuarioDataService;
    private readonly IRolDataService _rolDataService;

    public UsuarioService(
        IUsuarioDataService usuarioDataService,
        IRolDataService rolDataService)
    {
        _usuarioDataService = usuarioDataService;
        _rolDataService = rolDataService;
    }

    // -------------------------------------------------------------------------
    // Consultas
    // -------------------------------------------------------------------------

    public async Task<UsuarioResponse?> ObtenerPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var model = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException("Usuario", usuarioGuid);

        return UsuarioBusinessMapper.ToResponse(model);
    }

    public async Task<DataPagedResult<UsuarioResponse>> BuscarAsync(
        UsuarioFiltroRequest filtro,
        CancellationToken cancellationToken = default)
    {
        var filtroDataModel = new UsuarioFiltroDataModel
        {
            Termino = filtro.Termino,
            EstadoUsuario = filtro.EstadoUsuario,
            NombreRol = filtro.NombreRol,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize
        };

        var resultado = await _usuarioDataService
            .BuscarAsync(filtroDataModel, cancellationToken);

        return new DataPagedResult<UsuarioResponse>
        {
            Items = resultado.Items
                               .Select(UsuarioBusinessMapper.ToResponse)
                               .ToList(),
            PageNumber = resultado.PageNumber,
            PageSize = resultado.PageSize,
            TotalRecords = resultado.TotalRecords
        };
    }

    // -------------------------------------------------------------------------
    // Comandos
    // -------------------------------------------------------------------------

    public async Task<UsuarioResponse> CrearAsync(
        CrearUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar campos
        UsuarioValidator.ValidarCrear(request);

        // 2. Verificar unicidad de username y correo
        if (await _usuarioDataService.ExisteUsernameAsync(request.Username, cancellationToken))
            throw new ValidationException($"El username '{request.Username}' ya está en uso.");

        if (await _usuarioDataService.ExisteCorreoAsync(request.Correo, cancellationToken))
            throw new ValidationException($"El correo '{request.Correo}' ya está registrado.");

        // 3. Verificar que el rol solicitado existe
        if (await _rolDataService.ExisteNombreRolAsync(request.NombreRol, cancellationToken) == false)
            throw new NotFoundException("Rol", request.NombreRol);

        // 4. Generar hash y salt de contraseña
        var (hash, salt) = GenerarPasswordHash(request.Password);

        // 5. Mapear request a DataModel
        var dataModel = UsuarioBusinessMapper.ToDataModel(request);

        // 6. Persistir usuario
        var usuarioCreado = await _usuarioDataService
            .CrearAsync(dataModel, hash, salt, cancellationToken);

        // 7. Asignar rol automáticamente
        var roles = await _rolDataService.ObtenerTodosLosRolesAsync(cancellationToken);
        var rol = roles.FirstOrDefault(r => r.NombreRol == request.NombreRol);

        if (rol is not null)
            await _rolDataService.AsignarRolAsync(
                usuarioCreado.IdUsuario,
                rol.IdRol,
                request.CreadoPorUsuario,
                cancellationToken);

        // 8. Retornar response con roles actualizados
        var usuarioFinal = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioCreado.UsuarioGuid, cancellationToken);

        return UsuarioBusinessMapper.ToResponse(usuarioFinal!);
    }

    public async Task<UsuarioResponse> ActualizarAsync(
        ActualizarUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar campos
        UsuarioValidator.ValidarActualizar(request);

        // 2. Verificar que el usuario existe
        var existente = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", request.UsuarioGuid);

        // 3. Si cambia el username, verificar que no esté en uso por otro
        if (!string.Equals(existente.Username, request.Username,
                StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioDataService.ExisteUsernameAsync(request.Username, cancellationToken))
                throw new ValidationException($"El username '{request.Username}' ya está en uso.");
        }

        // 4. Si cambia el correo, verificar que no esté en uso por otro
        if (!string.Equals(existente.Correo, request.Correo,
                StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioDataService.ExisteCorreoAsync(request.Correo, cancellationToken))
                throw new ValidationException($"El correo '{request.Correo}' ya está registrado.");
        }

        // 5. Aplicar cambios
        existente.Username = request.Username.Trim();
        existente.Correo = request.Correo.Trim().ToLower();
        existente.ModificadoPorUsuario = request.ModificadoPorUsuario;

        var actualizado = await _usuarioDataService
            .ActualizarAsync(existente, cancellationToken)
            ?? throw new NotFoundException("Usuario", request.UsuarioGuid);

        return UsuarioBusinessMapper.ToResponse(actualizado);
    }

    public async Task EliminarLogicoAsync(
        Guid usuarioGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        // Verificar que existe antes de intentar eliminar
        var existente = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException("Usuario", usuarioGuid);

        var eliminado = await _usuarioDataService
            .EliminarLogicoAsync(existente.IdUsuario, modificadoPorUsuario, cancellationToken);

        if (!eliminado)
            throw new NotFoundException("Usuario", usuarioGuid);
    }

    // -------------------------------------------------------------------------
    // Helper privado — generación de hash de contraseña (HMACSHA256)
    // -------------------------------------------------------------------------

    private static (string hash, string salt) GenerarPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256();
        var saltBytes = hmac.Key;
        var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return (
            Convert.ToBase64String(hashBytes),
            Convert.ToBase64String(saltBytes)
        );
    }
}