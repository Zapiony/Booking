// Microservicio.Booking.Api/Controllers/V1/FacturacionController.cs

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.DTOs.Facturacion;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.DataManagement.Models;

namespace Microservicio.Booking.Api.Controllers.V1;

/// <summary>
/// Controlador para la gestión de facturación.
/// Expone endpoints para crear, consultar y anular facturas.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/facturacion")]
[Authorize]
public class FacturacionController : ControllerBase
{
    private readonly IFacturacionService _facturacionService;

    public FacturacionController(IFacturacionService facturacionService)
    {
        _facturacionService = facturacionService;
    }

    // -------------------------------------------------------------------------
    // GET /api/v1/facturacion/{guid}
    // -------------------------------------------------------------------------
    [HttpGet("{guidFactura:guid}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturacionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorGuid(
        Guid guidFactura,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionService.ObtenerPorGuidAsync(guidFactura, cancellationToken);
        return Ok(ApiResponse<FacturacionResponse>.Ok(result, "Consulta exitosa."));
    }

    // -------------------------------------------------------------------------
    // GET /api/v1/facturacion/numero/{numeroFactura}
    // -------------------------------------------------------------------------
    [HttpGet("numero/{numeroFactura}")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturacionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorNumero(
        string numeroFactura,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionService.ObtenerPorNumeroAsync(numeroFactura, cancellationToken);
        return Ok(ApiResponse<FacturacionResponse>.Ok(result, "Consulta exitosa."));
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/facturacion/buscar
    // -------------------------------------------------------------------------
    [HttpPost("buscar")]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<DataPagedResult<FacturacionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar(
        [FromBody] FacturacionFiltroRequest filtro,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionService.BuscarAsync(filtro, cancellationToken);
        return Ok(ApiResponse<DataPagedResult<FacturacionResponse>>.Ok(result, "Consulta paginada exitosa."));
    }

    // -------------------------------------------------------------------------
    // POST /api/v1/facturacion
    // -------------------------------------------------------------------------
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR,VENDEDOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturacionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearFacturacionRequest request,
        CancellationToken cancellationToken)
    {
        request.CreadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Booking.Api";

        var result = await _facturacionService.CrearAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorGuid),
            new { guidFactura = result.GuidFactura },
            ApiResponse<FacturacionResponse>.Ok(result, "Factura creada exitosamente."));
    }

    // -------------------------------------------------------------------------
    // PUT /api/v1/facturacion/{guid}
    // -------------------------------------------------------------------------
    [HttpPut("{guidFactura:guid}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<FacturacionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        Guid guidFactura,
        [FromBody] ActualizarFacturacionRequest request,
        CancellationToken cancellationToken)
    {
        request.GuidFactura = guidFactura;
        request.ModificadoPorUsuario = ObtenerUsernameDelToken();
        request.ModificacionIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        request.ServicioOrigen = "Microservicio.Booking.Api";

        var result = await _facturacionService.ActualizarAsync(request, cancellationToken);
        return Ok(ApiResponse<FacturacionResponse>.Ok(result, "Factura actualizada exitosamente."));
    }

    // -------------------------------------------------------------------------
    // DELETE /api/v1/facturacion/{guid}
    // -------------------------------------------------------------------------
    [HttpDelete("{guidFactura:guid}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarLogico(
        Guid guidFactura,
        CancellationToken cancellationToken)
    {
        var eliminadoPor = ObtenerUsernameDelToken();

        await _facturacionService.EliminarLogicoAsync(
            guidFactura,
            eliminadoPor,
            cancellationToken);

        return Ok(ApiResponse<string>.Ok("OK", "Factura eliminada lógicamente."));
    }

    private string ObtenerUsernameDelToken()
    {
        return User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.Identity?.Name
            ?? "sistema";
    }
}
