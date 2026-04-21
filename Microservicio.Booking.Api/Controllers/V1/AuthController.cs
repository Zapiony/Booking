using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Api.Models.Settings;
using Microservicio.Booking.Business.DTOs.Auth;
using Microservicio.Booking.Business.Interfaces;

namespace Microservicio.Booking.Api.Controllers.V1;

/// <summary>
/// Autenticación de usuarios.
/// Valida credenciales a través de AuthService (capa 3) y genera el JWT.
/// La generación del token es responsabilidad de esta capa — no de Business.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IAuthService authService, IOptions<JwtSettings> jwtOptions)
    {
        _authService = authService;
        _jwtSettings = jwtOptions.Value;
    }

    /// <summary>
    /// Autentica un usuario y devuelve el token JWT.
    /// </summary>
    /// <remarks>
    /// POST /api/v1/auth/login
    /// Body: { "username": "admin", "password": "mi_clave" }
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        // Business valida credenciales y devuelve datos del usuario
        var result = await _authService.LoginAsync(request, cancellationToken);

        // La API genera el JWT con los claims del usuario
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,        result.UsuarioGuid.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, result.Username),
            new(JwtRegisteredClaimNames.Email,      result.Correo),
        };

        // Un claim por cada rol — [Authorize(Roles = "ADMINISTRADOR")] los lee aquí
        claims.AddRange(result.Roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:            _jwtSettings.Issuer,
            audience:          _jwtSettings.Audience,
            claims:            claims,
            expires:           expiration,
            signingCredentials: credentials);

        result.Token         = new JwtSecurityTokenHandler().WriteToken(token);
        result.ExpirationUtc = expiration;

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso."));
    }
}
