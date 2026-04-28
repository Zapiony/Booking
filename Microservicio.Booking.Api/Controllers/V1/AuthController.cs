using Asp.Versioning;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Api.Models.Settings;
using Microservicio.Booking.Business.DTOs.Auth;
using Microservicio.Booking.Business.DTOs.Usuario;
using Microservicio.Booking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservicio.Booking.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsuarioService _usuarioService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        IAuthService authService,
        IUsuarioService usuarioService,
        IOptions<JwtSettings> jwtOptions)
    {
        _authService = authService;
        _usuarioService = usuarioService;
        _jwtSettings = jwtOptions.Value;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,        result.UsuarioGuid.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, result.Username),
            new(JwtRegisteredClaimNames.Email,      result.Correo),
        };

        claims.AddRange(result.Roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        result.Token = new JwtSecurityTokenHandler().WriteToken(token);
        result.ExpirationUtc = expiration;

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso."));
    }

    [HttpPost("registro")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registro(
    [FromBody] CrearUsuarioRequest request,
    CancellationToken cancellationToken)
    {
        // Registro público — sin token, el mismo correo es el origen
        request.CreadoPorUsuario = request.Correo ?? "self";

        var result = await _usuarioService.CrearAsync(request, cancellationToken);
        return StatusCode(201, ApiResponse<UsuarioResponse>.Ok(result, "Usuario creado exitosamente."));
    }
}