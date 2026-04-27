using Asp.Versioning.ApiExplorer;
using Microservicio.Booking.Api.Extensions;
using Microservicio.Booking.Api.Middleware;
using Microservicio.Booking.Api.Models.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// -------------------------------------------------------------------------
// Configuraciones transversales
// -------------------------------------------------------------------------
builder.Services.AddBookingApiVersioning();
builder.Services.AddBookingCors(builder.Configuration);
builder.Services.AddBookingJwtAuthentication(builder.Configuration);
builder.Services.AddBookingSwagger();

// -------------------------------------------------------------------------
// Módulos de negocio
// Cada módulo del equipo registra sus propios servicios aquí.
// -------------------------------------------------------------------------
builder.Services.AddBookingApplicationServices(builder.Configuration);

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

app.Run();
