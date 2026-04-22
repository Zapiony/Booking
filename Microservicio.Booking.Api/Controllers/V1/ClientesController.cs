// Microservicio.Booking.Api/Controllers/V1/ClientesController.cs

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.DTOs.Cliente;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Api.Controllers.V1;

/// <summary>
/// CRUD de clientes del sistema.
/// Requiere autenticación JWT. La eliminación lógica
/// está restringida al rol ADMINISTRADOR.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/clientes")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    // -------------------------------------------------------------------------
    // GET /api/v1/clientes/{guid}
    // -------------------------------------------------------------------------

    /// <summary>
    /// Obtiene un cliente por su GUID público.
    /// </summary>
    [HttpGet("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorGuid(
        Guid guidCliente,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.ObtenerPorGuidAsync(guidCliente, cancellationToken);
        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Consulta exitosa."));
    }

    // -------------------------------------------------------------------------
    // GET /api/v1/clientes/identificacion/{tipo}/{numero}
    // -------------------------------------------------------------------------

    /// <summary>
    /// Obtiene un cliente por tipo y número de identificación.
    /// </summary>
    [HttpGet("identificacion/{tipoIdentificacion}/{numeroIdentificacion}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorIdentificacion(
        string tipoIdentificacion,
        string numeroIdentificacion,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.ObtenerPorNumeroIdentificacionAsync(
            tipoIdentificacion,
            numeroIdentificacion,
            cancellationToken);

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Consulta exitosa."));
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/clientes/buscar
    // -------------------------------------------------------------------------

    /// <summary>
    /// Búsqueda paginada de clientes con filtros opcionales.
    /// </summary>
    [HttpPost("buscar")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<ClienteResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar(
        [FromBody] ClienteFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _clienteService.BuscarAsync(filtro, cancellationToken);
        return Ok(ApiResponse<DataPagedResult<ClienteResponse>>.Ok(result, "Consulta paginada exitosa."));
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/clientes
    // -------------------------------------------------------------------------

    /// <summary>
    /// Registra un nuevo cliente en el sistema.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearClienteRequest request,
        CancellationToken cancellationToken)
    {
        // Auditoría: se toma del token, no del body
        request.CreadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Booking.Api";

        var result = await _clienteService.CrearAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorGuid),
            new { guidCliente = result.GuidCliente },
            ApiResponse<ClienteResponse>.Ok(result, "Cliente creado exitosamente."));
    }

    // -------------------------------------------------------------------------
    // PUT /api/v1/clientes/{guid}
    // -------------------------------------------------------------------------

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    [HttpPut("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        Guid guidCliente,
        [FromBody] ActualizarClienteRequest request,
        CancellationToken cancellationToken)
    {
        request.GuidCliente = guidCliente;
        request.ModificadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Booking.Api";

        var result = await _clienteService.ActualizarAsync(request, cancellationToken);
        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Cliente actualizado exitosamente."));
    }

    // -------------------------------------------------------------------------
    // PATCH /api/v1/clientes/{guid}/estado/{nuevoEstado}
    // -------------------------------------------------------------------------

    /// <summary>
    /// Cambia el estado de un cliente (ACT / INA / SUS).
    /// </summary>
    [HttpPatch("{guidCliente:guid}/estado/{nuevoEstado}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<ClienteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(
        Guid guidCliente,
        string nuevoEstado,
        CancellationToken cancellationToken)
    {
        var modificadoPor = ObtenerUsernameDelToken();

        var result = await _clienteService.CambiarEstadoAsync(
            guidCliente,
            nuevoEstado,
            modificadoPor,
            cancellationToken);

        return Ok(ApiResponse<ClienteResponse>.Ok(result, "Estado actualizado exitosamente."));
    }

    // -------------------------------------------------------------------------
    // DELETE /api/v1/clientes/{guid}
    // -------------------------------------------------------------------------

    /// <summary>
    /// Eliminación lógica de un cliente. Solo ADMINISTRADOR puede ejecutarla.
    /// </summary>
    [HttpDelete("{guidCliente:guid}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarLogico(
        Guid guidCliente,
        CancellationToken cancellationToken)
    {
        var eliminadoPor = ObtenerUsernameDelToken();

        await _clienteService.EliminarLogicoAsync(
            guidCliente,
            eliminadoPor,
            cancellationToken);

        return Ok(ApiResponse<string>.Ok("OK", "Cliente eliminado lógicamente."));
    }

    // -------------------------------------------------------------------------
    // Helper privado
    // -------------------------------------------------------------------------

    private string ObtenerUsernameDelToken()
    {
        return User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.Identity?.Name
            ?? "sistema";
    }
}