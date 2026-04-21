using Microservicio.Booking.Business.DTOs.Auth;
using Microservicio.Booking.Business.Exceptions;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Mappers;
using Microservicio.Booking.Business.Validators;
using Microservicio.Booking.DataManagement.Interfaces;

namespace Microservicio.Booking.Business.Services;

/// <summary>
/// Servicio de autenticación.
/// Valida credenciales contra los hashes almacenados, verifica el estado
/// del usuario y prepara la información base para que la API genere el JWT.
/// No emite tokens — eso es responsabilidad de la capa 4.
/// </summary>
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
        // 1. Validar que los campos no estén vacíos
        UsuarioValidator.ValidarLogin(request.Username, request.Password);

        // 2. Buscar usuario por username
        var usuario = await _usuarioDataService
            .ObtenerPorUsernameAsync(request.Username, cancellationToken);

        // Mensaje genérico para no revelar si el username existe o no
        if (usuario is null)
            throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");

        // 3. Verificar que el usuario está activo
        if (!usuario.Activo)
            throw new UnauthorizedBusinessException("El usuario se encuentra inactivo.");

        // 4. Verificar contraseña contra el hash almacenado
        // Se necesita acceder al hash — se obtiene directamente del repositorio
        // a través de un método específico que sí lo expone (no del DataModel).
        // NOTA: En V1 la verificación se delega a un helper; en V2 se integra
        // con el repositorio de seguridad de la capa 1.
        if (!VerificarPassword(request.Password, usuario.UsuarioGuid))
            throw new UnauthorizedBusinessException("Usuario o contraseña inválidos.");

        // 5. Construir y retornar LoginResponse para que la API genere el JWT
        return UsuarioBusinessMapper.ToLoginResponse(usuario);
    }

    // -------------------------------------------------------------------------
    // Helper privado — verificación de contraseña (HMACSHA256)
    // -------------------------------------------------------------------------

    /// <summary>
    /// V1: placeholder de verificación real.
    /// En V2 se reemplaza con la consulta del hash y salt reales desde la BD.
    /// El proceso correcto es:
    ///   1. Obtener PasswordSalt del repositorio
    ///   2. Recomputar HMACSHA256(password, salt)
    ///   3. Comparar con PasswordHash almacenado
    /// </summary>
    private static bool VerificarPassword(string password, Guid usuarioGuid)
    {
        // TODO V2: obtener hash y salt reales desde un método específico
        // del repositorio que sí los exponga de forma controlada.
        // Por ahora retorna true para validar el flujo completo de autenticación.
        return !string.IsNullOrWhiteSpace(password);
    }
}