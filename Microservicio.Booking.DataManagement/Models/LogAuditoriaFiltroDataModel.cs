namespace Microservicio.Booking.DataManagement.Models;

public class LogAuditoriaFiltroDataModel
{
    public string? TablaAfectada { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TamanioPagina { get; set; } = 10;
}
