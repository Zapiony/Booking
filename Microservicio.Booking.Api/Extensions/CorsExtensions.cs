namespace Microservicio.Booking.Api.Extensions;

/// <summary>
/// Configura CORS para permitir peticiones desde los frontends locales.
/// Los orígenes se leen desde appsettings.json → sección "Cors:AllowedOrigins".
/// </summary>
public static class CorsExtensions
{
    public static IServiceCollection AddCustomCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
