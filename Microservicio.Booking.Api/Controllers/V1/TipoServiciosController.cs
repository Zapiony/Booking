using Asp.Versioning;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.DTOs.TipoServicio;
using Microservicio.Booking.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Booking.Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/tipos-servicio")]
[AllowAnonymous]
public sealed class TipoServiciosController : ControllerBase
{
    private readonly ITipoServicioService _tiposServicio;

    public TipoServiciosController(ITipoServicioService tiposServicio)
    {
        _tiposServicio = tiposServicio;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> ListarPaginadoAsync(
        [FromQuery] int paginaActual = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _tiposServicio.ListarPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);
        return Ok(ApiResponse<object>.Exitoso(resultado, "Consulta exitosa"));
    }

    [HttpGet("activos")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TipoServicioResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TipoServicioResponse>>>> ListarActivosAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await _tiposServicio.ListarActivosAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<TipoServicioResponse>>.Exitoso(items, "Consulta exitosa"));
    }

    [HttpGet("por-nombre")]
    [ProducesResponseType(typeof(ApiResponse<TipoServicioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorNombreAsync(
        [FromQuery] string nombre,
        CancellationToken cancellationToken = default)
    {
        var data = await _tiposServicio.ObtenerPorNombreAsync(nombre, cancellationToken);
        if (data is null)
            return NotFound(ApiErrorResponse.Crear("Tipo de servicio no encontrado.", new[] { $"No existe tipo con nombre '{nombre}'." }));

        return Ok(ApiResponse<TipoServicioResponse>.Exitoso(data, "Consulta exitosa"));
    }

    [HttpGet("{guid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TipoServicioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var data = await _tiposServicio.ObtenerPorGuidAsync(guid, cancellationToken);
        if (data is null)
            return NotFound(ApiErrorResponse.Crear("Tipo de servicio no encontrado.", new[] { $"No existe tipo con Guid {guid}." }));

        return Ok(ApiResponse<TipoServicioResponse>.Exitoso(data, "Consulta exitosa"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TipoServicioResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<TipoServicioResponse>>> CrearAsync(
        [FromBody] CrearTipoServicioRequest request,
        CancellationToken cancellationToken = default)
    {
        var creado = await _tiposServicio.CrearAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<TipoServicioResponse>.Exitoso(creado, "Tipo de servicio creado."));
    }

    [HttpPut("{guid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TipoServicioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TipoServicioResponse>>> ActualizarAsync(
        Guid guid,
        [FromBody] ActualizarTipoServicioRequest request,
        CancellationToken cancellationToken = default)
    {
        request.GuidTipoServicio = guid;
        var actualizado = await _tiposServicio.ActualizarAsync(request, cancellationToken);
        return Ok(ApiResponse<TipoServicioResponse>.Exitoso(actualizado, "Tipo de servicio actualizado."));
    }

    [HttpDelete("{guid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EliminarAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        await _tiposServicio.EliminarAsync(guid, cancellationToken);
        return NoContent();
    }
}
