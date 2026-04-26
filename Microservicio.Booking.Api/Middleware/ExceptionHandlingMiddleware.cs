using System.Net;
using System.Text.Json;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.Exceptions;

namespace Microservicio.Booking.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado: {Mensaje}", ex.Message);
            await EscribirRespuestaAsync(context, ex);
        }
    }

    private static async Task EscribirRespuestaAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

<<<<<<< Updated upstream
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
=======
        var (statusCode, body) = exception switch
        {
            ValidationException ve => (
                (int)HttpStatusCode.BadRequest,
                ApiErrorResponse.Crear(ve.Message, ve.Errors.ToList())),
            NotFoundException ne => (
                (int)HttpStatusCode.NotFound,
                ApiErrorResponse.Crear(ne.Message, new[] { ne.Message })),
            BusinessException be => (
                (int)HttpStatusCode.Conflict,
                ApiErrorResponse.Crear(be.Message, new[] { be.Message })),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                ApiErrorResponse.Crear("Error interno del servidor.", Array.Empty<string>()))
        };
>>>>>>> Stashed changes

        await context.Response.WriteAsync(json);
    }
}
