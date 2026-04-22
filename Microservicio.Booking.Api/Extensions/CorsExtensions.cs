using Microservicio.Booking.Api.Models.Settings;

namespace Microservicio.Booking.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "BookingCorsPolicy";

    public static IServiceCollection AddBookingCors(this IServiceCollection services, IConfiguration configuration)
    {
        var cors = configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                if (cors.AllowedOrigins is { Length: > 0 })
                    policy.WithOrigins(cors.AllowedOrigins).AllowAnyHeader().AllowAnyMethod();
                else
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        return services;
    }
}
