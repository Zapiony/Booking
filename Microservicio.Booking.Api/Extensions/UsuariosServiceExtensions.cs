using Microsoft.EntityFrameworkCore;
using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Services;
using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Services;

namespace Microservicio.Booking.Api.Extensions;

/// <summary>
/// Registra todos los servicios del módulo de Usuarios (capas 1, 2 y 3).
/// Se llama desde Program.cs con builder.Services.AddUsuariosModule(configuration).
/// Cada módulo del equipo tiene su propia extension de este tipo.
/// </summary>
public static class UsuariosServiceExtensions
{
    public static IServiceCollection AddUsuariosModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Capa 1 — DbContext (PostgreSQL)
        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BookingDb")));

        // Capa 2 — Unit of Work y servicios de datos
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsuarioDataService, UsuarioDataService>();
        services.AddScoped<IRolDataService, RolDataService>();

        // Capa 3 — Servicios de negocio
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
