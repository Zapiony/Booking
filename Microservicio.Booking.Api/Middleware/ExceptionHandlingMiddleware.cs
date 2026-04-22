using System.Net;
using System.Text.Json;
using Microservicio.Booking.Api.Models.Common;
using Microservicio.Booking.Business.Exceptions;

namespace Microservicio.Booking.Api.Middleware;

/// <summary>
/// Middleware global de manejo de excepciones.
/// Captura cualquier excepción no controlada del pipeline y la traduce
/// a una respuesta HTTP estructurada con ApiErrorResponse.
/// Evita duplicar try/catch en cada controller.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest,
                ApiErrorResponse.Fail(ex.Message, ex.Errors));
        }
        catch (NotFoundException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.NotFound,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (UnauthorizedBusinessException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (BusinessException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest,
                ApiErrorResponse.Fail(ex.Message));
        }
        catch (Exception)
        {
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError,
                ApiErrorResponse.Fail("Ha ocurrido un error interno en el servidor."));
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        ApiErrorResponse response)
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
