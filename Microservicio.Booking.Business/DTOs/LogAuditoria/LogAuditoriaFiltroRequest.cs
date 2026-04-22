namespace Microservicio.Booking.Business.DTOs.LogAuditoria;

public class LogAuditoriaFiltroRequest
{
    public string? TablaAfectada { get; set; }
    public string? Operacion { get; set; }
    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset? FechaInicioUtc { get; set; }
    public DateTimeOffset? FechaFinUtc { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
