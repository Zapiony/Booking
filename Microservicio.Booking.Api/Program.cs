using Asp.Versioning.ApiExplorer;
using Microservicio.Booking.Api.Extensions;
using Microservicio.Booking.Api.Middleware;
using Microservicio.Booking.Api.Models.Settings;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddBookingApiVersioning();
builder.Services.AddBookingSwagger();
builder.Services.AddBookingCors(builder.Configuration);
builder.Services.AddBookingJwtAuthentication(builder.Configuration);
builder.Services.AddBookingApplicationServices(builder.Configuration);

builder.Services.AddRazorPages();

<<<<<<< feat/DataAccess_Servicios
=======
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
builder.Services.AddClientesModule();
// builder.Services.AddFacturacionModule(builder.Configuration);
// builder.Services.AddServiciosModule(builder.Configuration);

// -------------------------------------------------------------------------
// Pipeline HTTP
// -------------------------------------------------------------------------
>>>>>>> master
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsExtensions.PolicyName);

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
if (jwtSettings?.Enabled == true)
    app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
