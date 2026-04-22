using Microservicio.Booking.Api.Extensions;
using Microservicio.Booking.Api.Middleware;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// Servicios base
// -------------------------------------------------------------------------
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services.AddControllers();

// -------------------------------------------------------------------------
// Configuraciones transversales
// -------------------------------------------------------------------------
builder.Services.AddCustomApiVersioning();
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddAuthorization();

// -------------------------------------------------------------------------
// Módulos de negocio
// Cada módulo del equipo registra sus propios servicios aquí.
// -------------------------------------------------------------------------
builder.Services.AddUsuariosModule(builder.Configuration);

// TODO: otros módulos del equipo se agregan aquí:
// builder.Services.AddClientesModule(builder.Configuration);
// builder.Services.AddFacturacionModule(builder.Configuration);
// builder.Services.AddServiciosModule(builder.Configuration);

// -------------------------------------------------------------------------
// Pipeline HTTP
// -------------------------------------------------------------------------
var app = builder.Build();

// Middleware global de errores — debe ser el primero del pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
