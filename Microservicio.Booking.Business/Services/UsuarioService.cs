using Microservicio.Booking.Business.DTOs.Usuario;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Business.Services;

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

    public async Task<UsuarioResponse?> ObtenerPorGuidAsync(
        Guid usuarioGuid,
        CancellationToken cancellationToken = default)
    {
        var model = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException($"No se encontró el usuario con GUID '{usuarioGuid}'.");

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
            PaginaActual = filtro.PageNumber,
            TamanioPagina = filtro.PageSize
        };

        var resultado = await _usuarioDataService
            .BuscarAsync(filtroDataModel, cancellationToken);

        var items = resultado.Items
                             .Select(UsuarioBusinessMapper.ToResponse)
                             .ToList();

        return new DataPagedResult<UsuarioResponse>(
            items,
            resultado.TotalRegistros,
            resultado.PaginaActual,
            resultado.TamanoPagina);
    }

    public async Task<UsuarioResponse> CrearAsync(
        CrearUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        UsuarioValidator.ValidarCrear(request);

        if (await _usuarioDataService.ExisteUsernameAsync(request.Username, cancellationToken))
            throw new ValidationException($"El username '{request.Username}' ya está en uso.");

        if (await _usuarioDataService.ExisteCorreoAsync(request.Correo, cancellationToken))
            throw new ValidationException($"El correo '{request.Correo}' ya está registrado.");

        if (await _rolDataService.ExisteNombreRolAsync(request.NombreRol, cancellationToken) == false)
            throw new NotFoundException($"No se encontró el rol '{request.NombreRol}'.");

        var (hash, salt) = GenerarPasswordHash(request.Password);
        var dataModel = UsuarioBusinessMapper.ToDataModel(request);
        var usuarioCreado = await _usuarioDataService
            .CrearAsync(dataModel, hash, salt, cancellationToken);

        var roles = await _rolDataService.ObtenerTodosLosRolesAsync(cancellationToken);
        var rol = roles.FirstOrDefault(r => r.NombreRol == request.NombreRol);

        if (rol is not null)
            await _rolDataService.AsignarRolAsync(
                usuarioCreado.IdUsuario,
                rol.IdRol,
                request.CreadoPorUsuario,
                cancellationToken);

        var usuarioFinal = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioCreado.UsuarioGuid, cancellationToken);

        return UsuarioBusinessMapper.ToResponse(usuarioFinal!);
    }

    public async Task<UsuarioResponse> ActualizarAsync(
        ActualizarUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        UsuarioValidator.ValidarActualizar(request);

        var existente = await _usuarioDataService
            .ObtenerPorGuidAsync(request.UsuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{request.UsuarioGuid}'.");

        if (!string.Equals(existente.Username, request.Username, StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioDataService.ExisteUsernameAsync(request.Username, cancellationToken))
                throw new ValidationException($"El username '{request.Username}' ya está en uso.");
        }

        if (!string.Equals(existente.Correo, request.Correo, StringComparison.OrdinalIgnoreCase))
        {
            if (await _usuarioDataService.ExisteCorreoAsync(request.Correo, cancellationToken))
                throw new ValidationException($"El correo '{request.Correo}' ya está registrado.");
        }

        existente.Username = request.Username.Trim();
        existente.Correo = request.Correo.Trim().ToLower();
        existente.ModificadoPorUsuario = request.ModificadoPorUsuario;

        var actualizado = await _usuarioDataService
            .ActualizarAsync(existente, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{request.UsuarioGuid}'.");

        return UsuarioBusinessMapper.ToResponse(actualizado);
    }

    public async Task EliminarLogicoAsync(
        Guid usuarioGuid,
        string modificadoPorUsuario,
        CancellationToken cancellationToken = default)
    {
        var existente = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken)
            ?? throw new NotFoundException($"No se encontró el usuario con GUID '{usuarioGuid}'.");

        var eliminado = await _usuarioDataService
            .EliminarLogicoAsync(existente.IdUsuario, modificadoPorUsuario, cancellationToken);

        if (!eliminado)
            throw new NotFoundException($"No se pudo eliminar el usuario con GUID '{usuarioGuid}'.");
    }

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
    public async Task<int> ObtenerIdInternoAsync(
    Guid usuarioGuid,
    CancellationToken cancellationToken = default)
    {
        var model = await _usuarioDataService
            .ObtenerPorGuidAsync(usuarioGuid, cancellationToken);

        if (model is null)
            throw new NotFoundException($"No se encontró el usuario con GUID '{usuarioGuid}'.");

        return model.IdUsuario;
    }
}