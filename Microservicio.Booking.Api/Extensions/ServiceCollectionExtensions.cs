using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Services;
using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra DbContext, capa DataManagement y capa Business para Servicio / TipoServicio.
    /// </summary>
    public static IServiceCollection AddBookingApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Default en configuración.");

        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<BookingDbContext>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServicioDataService, ServicioDataService>();
        services.AddScoped<ITipoServicioDataService, TipoServicioDataService>();

        services.AddScoped<IServicioService, ServicioService>();
        services.AddScoped<ITipoServicioService, TipoServicioService>();

        return services;
    }
}
