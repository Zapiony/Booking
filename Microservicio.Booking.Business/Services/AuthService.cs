using Microservicio.Booking.Business.DTOs.Auth;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioDataService _usuarioDataService;

    public AuthService(IUsuarioDataService usuarioDataService)
    {
        _usuarioDataService = usuarioDataService;
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        UsuarioValidator.ValidarLogin(request.Username, request.Password);

        // 1. Buscar usuario (sin hash — UsuarioDataModel no lo expone)
        var usuario = await _usuarioDataService
            .ObtenerPorUsernameAsync(request.Username, cancellationToken);

        if (usuario is null)
            throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");

        if (!usuario.Activo)
            throw new UnauthorizedBusinessException("El usuario se encuentra inactivo.");

        // 2. Obtener credenciales solo para verificación
        var credenciales = await _usuarioDataService
            .ObtenerCredencialesParaAuthAsync(request.Username, cancellationToken);

        if (credenciales is null)
            throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");

        // 3. Verificar contraseña contra el hash almacenado
        if (!VerificarPassword(request.Password, credenciales.Value.PasswordHash, credenciales.Value.PasswordSalt))
            throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");

        return UsuarioBusinessMapper.ToLoginResponse(usuario);
    }

    // -------------------------------------------------------------------------
    // Helper privado — HMACSHA256 con el salt almacenado
    // -------------------------------------------------------------------------
    private static bool VerificarPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        using var hmac = new System.Security.Cryptography.HMACSHA256(saltBytes);
        var computedHash = Convert.ToBase64String(
            hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        return computedHash == storedHash;
    }
}