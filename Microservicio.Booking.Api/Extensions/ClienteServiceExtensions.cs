// Microservicio.Booking.Api/Extensions/ClientesServiceExtensions.cs

using Microservicio.Booking.Business.Interfaces;
using Microservicio.Booking.Business.Services;
using Microservicio.Booking.DataManagement.Interfaces;
using Microservicio.Booking.DataManagement.Services;

namespace Microservicio.Booking.Api.Extensions;

/// <summary>
/// Registra todos los servicios del módulo de Clientes (capas 2 y 3).
/// Se llama desde Program.cs con builder.Services.AddClientesModule().
/// El DbContext y la UoW ya los registra AddUsuariosModule — no se duplican.
/// </summary>
public static class ClientesServiceExtensions
{
    public static IServiceCollection AddClientesModule(
        this IServiceCollection services)
    {
        // Capa 2 — Servicio de datos
        services.AddScoped<IClienteDataService, ClienteDataService>();

        // Capa 3 — Servicio de negocio
        services.AddScoped<IClienteService, ClienteService>();

        return services;
    }
}