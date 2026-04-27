using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Services;
using Microservicio.Booking.DataAccess.Context;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.Booking.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BookingDb")
            ?? configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:BookingDb o ConnectionStrings:Default.");

        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServicioDataService, ServicioDataService>();
        services.AddScoped<ITipoServicioDataService, TipoServicioDataService>();

        services.AddScoped<IServicioService, ServicioService>();
        services.AddScoped<ITipoServicioService, TipoServicioService>();

        return services;
    }
}
