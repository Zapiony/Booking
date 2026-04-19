using Asp.Versioning;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.DTOs.Servicio;
using Microservicio.Booking.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Booking.Api.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/servicios")]
[AllowAnonymous]
public sealed class ServiciosController : ControllerBase
{
    private readonly IServicioService _servicios;

    public ServiciosController(IServicioService servicios)
    {
        _servicios = servicios;
    }

    /// <summary>Listado paginado con filtros (término, tipo de servicio).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> ListarAsync(
        [FromQuery] string? termino,
        [FromQuery] Guid? guidTipo,
        [FromQuery] int paginaActual = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var filtro = new ServicioFiltroRequest
        {
            Termino = termino,
            GuidTipoServicio = guidTipo,
            PaginaActual = paginaActual,
            TamanoPagina = tamanoPagina
        };

        var resultado = await _servicios.ListarOBuscarAsync(filtro, cancellationToken);
        return Ok(ApiResponse<object>.Exitoso(resultado, "Consulta exitosa"));
    }

    /// <summary>Paginación sobre entidades completas (repositorio).</summary>
    [HttpGet("pagina-completa")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> ListarPaginaCompletaAsync(
        [FromQuery] int paginaActual = 1,
        [FromQuery] int tamanoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _servicios.ListarEntidadesPaginadoAsync(paginaActual, tamanoPagina, cancellationToken);
        return Ok(ApiResponse<object>.Exitoso(resultado, "Consulta exitosa"));
    }

    [HttpGet("{guid:guid}/detalle")]
    [ProducesResponseType(typeof(ApiResponse<ServicioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var data = await _servicios.ObtenerDetallePorGuidAsync(guid, cancellationToken);
        if (data is null)
            return NotFound(ApiErrorResponse.Crear("Servicio no encontrado.", new[] { $"No existe servicio con Guid {guid}." }));

        return Ok(ApiResponse<ServicioResponse>.Exitoso(data, "Consulta exitosa"));
    }

    [HttpGet("{guid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServicioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var data = await _servicios.ObtenerPorGuidAsync(guid, cancellationToken);
        if (data is null)
            return NotFound(ApiErrorResponse.Crear("Servicio no encontrado.", new[] { $"No existe servicio con Guid {guid}." }));

        return Ok(ApiResponse<ServicioResponse>.Exitoso(data, "Consulta exitosa"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ServicioResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ServicioResponse>>> CrearAsync(
        [FromBody] CrearServicioRequest request,
        CancellationToken cancellationToken = default)
    {
        var creado = await _servicios.CrearAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<ServicioResponse>.Exitoso(creado, "Servicio creado."));
    }

    [HttpPut("{guid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServicioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ServicioResponse>>> ActualizarAsync(
        Guid guid,
        [FromBody] ActualizarServicioRequest request,
        CancellationToken cancellationToken = default)
    {
        request.GuidServicio = guid;
        var actualizado = await _servicios.ActualizarAsync(request, cancellationToken);
        return Ok(ApiResponse<ServicioResponse>.Exitoso(actualizado, "Servicio actualizado."));
    }

    [HttpDelete("{guid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EliminarAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        await _servicios.EliminarAsync(guid, cancellationToken);
        return NoContent();
    }
}
