using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microservicio.Booking.Api.Models.Settings;

namespace Microservicio.Booking.Api.Extensions;

/// <summary>
/// Configura autenticación JWT Bearer.
/// Lee JwtSettings desde appsettings.json y registra el esquema.
/// </summary>
public static class AuthenticationExtensions
{
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                "No se encontró la configuración JwtSettings en appsettings.json.");

        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // Registra JwtSettings para IOptions<JwtSettings> en controllers
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}
